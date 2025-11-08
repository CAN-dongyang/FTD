using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PathfindingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<PathfindingParams>(out var pathfindingParams))
            return;
            
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        state.Dependency = new PathfindingJob
        {
            PathfindingParams = pathfindingParams,
            Ecb = ecb,
        }.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    [WithAll(typeof(PathRequest))]
    public partial struct PathfindingJob : IJobEntity
    {
        [ReadOnly] public PathfindingParams PathfindingParams;
        public EntityCommandBuffer.ParallelWriter Ecb;

        private void Execute(Entity entity, [ChunkIndexInQuery] int chunkIndex, in PathRequest request)
        {
            var startGridPos = WorldToGrid(request.StartPosition);
            var endGridPos = WorldToGrid(request.EndPosition);

            var path = FindPath(startGridPos, endGridPos);

            if (path.Length > 0)
            {
                var pathBuffer = Ecb.AddBuffer<PathWaypoint>(chunkIndex, entity);
                for(int i = 0; i < path.Length; i++)
                {
                    pathBuffer.Add(new PathWaypoint { Position = GridToWorld(path[i]) });
                }
                Ecb.SetComponent(chunkIndex, entity, new CurrentPathIndex { Value = 0 });
            }
            
            path.Dispose();
            Ecb.RemoveComponent<PathRequest>(chunkIndex, entity);
        }

        private NativeList<int2> FindPath(int2 startPos, int2 endPos)
        {
            var openList = new NativeList<int>(Allocator.Temp);
            var closedList = new NativeHashSet<int>(1000, Allocator.Temp);
            var cameFrom = new NativeHashMap<int, int>(1000, Allocator.Temp);
            var gScore = new NativeHashMap<int, int>(1000, Allocator.Temp);
            var fScore = new NativeHashMap<int, int>(1000, Allocator.Temp);

            int startIndex = GridToIndex(startPos);
            int endIndex = GridToIndex(endPos);

            gScore[startIndex] = 0;
            fScore[startIndex] = Heuristic(startPos, endPos);
            openList.Add(startIndex);
            
            ref var grid = ref PathfindingParams.GridBlob.Value.Grid;

            while(openList.Length > 0)
            {
                int currentIndex = GetLowestFScore(openList, fScore);
                var currentPos = IndexToGrid(currentIndex);

                if(currentIndex == endIndex)
                {
                    var finalPath = ReconstructPath(cameFrom, currentIndex);
                    openList.Dispose();
                    closedList.Dispose();
                    cameFrom.Dispose();
                    gScore.Dispose();
                    fScore.Dispose();
                    return finalPath;
                }

                openList.RemoveAtSwapBack(GetIndex(openList, currentIndex));
                closedList.Add(currentIndex);

                for(int i = 0; i < 4; i++)
                {
                    var neighborPos = currentPos + GetDirection(i);
                    
                    if (!IsWalkable(neighborPos) || closedList.Contains(GridToIndex(neighborPos)))
                    {
                        continue;
                    }

                    int tentativeGScore = gScore[currentIndex] + 1;
                    int neighborIndex = GridToIndex(neighborPos);
                    
                    if(!gScore.ContainsKey(neighborIndex) || tentativeGScore < gScore[neighborIndex])
                    {
                        cameFrom[neighborIndex] = currentIndex;
                        gScore[neighborIndex] = tentativeGScore;
                        fScore[neighborIndex] = tentativeGScore + Heuristic(neighborPos, endPos);
                        if(!openList.Contains(neighborIndex))
                        {
                            openList.Add(neighborIndex);
                        }
                    }
                }
            }
            
            openList.Dispose();
            closedList.Dispose();
            cameFrom.Dispose();
            gScore.Dispose();
            fScore.Dispose();
            return new NativeList<int2>(Allocator.Temp);
        }

        // --- 여기가 수정되었습니다 ---
        private NativeList<int2> ReconstructPath(in NativeHashMap<int, int> cameFrom, int currentIndex)
        {
            var path = new NativeList<int2>(Allocator.Temp);
            path.Add(IndexToGrid(currentIndex)); 
            
            // cameFrom.ContainsKey 대신 더 안전한 TryGetValue를 사용합니다.
            while(cameFrom.TryGetValue(currentIndex, out int previousIndex))
            {
                currentIndex = previousIndex;
                path.Add(IndexToGrid(currentIndex));
            }
            
            var reversedPath = new NativeList<int2>(path.Length, Allocator.Temp);
            for(int i = path.Length-1; i >= 0; i--)
            {
                reversedPath.Add(path[i]);
            }
            path.Dispose();
            return reversedPath;
        }

        private bool IsWalkable(int2 gridPosition)
        {
            int2 relativePos = gridPosition - PathfindingParams.GridOrigin;
            if (relativePos.x < 0 || relativePos.x >= PathfindingParams.MapSize.x || relativePos.y < 0 || relativePos.y >= PathfindingParams.MapSize.y) return false;
            int index = relativePos.y * PathfindingParams.MapSize.x + relativePos.x;
            return !PathfindingParams.GridBlob.Value.Grid[index];
        }

        private int Heuristic(int2 a, int2 b) => math.abs(a.x - b.x) + math.abs(a.y - b.y);
        private int2 WorldToGrid(float3 worldPosition) => new int2(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));
        private float3 GridToWorld(int2 gridPosition) => new float3(gridPosition.x + 0.5f, gridPosition.y + 0.5f, 0);
        private int GridToIndex(int2 gridPosition) => (gridPosition.y - PathfindingParams.GridOrigin.y) * PathfindingParams.MapSize.x + (gridPosition.x - PathfindingParams.GridOrigin.x);
        private int2 IndexToGrid(int index)
        {
            int y = index / PathfindingParams.MapSize.x;
            int x = index % PathfindingParams.MapSize.x;
            return new int2(x, y) + PathfindingParams.GridOrigin;
        }
        private int2 GetDirection(int i) => i switch { 0 => new int2(0, 1), 1 => new int2(0, -1), 2 => new int2(-1, 0), _ => new int2(1, 0) };
        private int GetLowestFScore(NativeList<int> list, NativeHashMap<int, int> fScore)
        {
            int lowestIndex = list[0];
            for(int i=1; i < list.Length; i++) { if (fScore.IsCreated && fScore.ContainsKey(list[i]) && fScore.ContainsKey(lowestIndex) && fScore[list[i]] < fScore[lowestIndex]) lowestIndex = list[i]; }
            return lowestIndex;
        }
        private int GetIndex(NativeList<int> list, int value)
        {
            for(int i=0; i < list.Length; i++) { if (list[i] == value) return i; }
            return -1;
        }
    }
}

