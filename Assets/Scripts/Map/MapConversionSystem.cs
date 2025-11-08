using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Burst;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct MapConversionSystem : ISystem
{
    [BurstDiscard]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.HasSingleton<MapConvertedTag>())
        {
            state.Enabled = false;
            return;
        }

        // --- 여기가 수정되었습니다 ---
        // MapManager가 존재하고, IsInitialized '초록불'이 켜졌는지 확인합니다.
        if (MapManager.Instance == null || !MapManager.Instance.IsInitialized || MapManager.Instance.collisionTilemaps.Count == 0)
        {
            // 아직 준비되지 않았다면, 다음 프레임에 다시 확인합니다.
            return;
        }
        
        var collisionTilemap = MapManager.Instance.collisionTilemaps[0];
        collisionTilemap.CompressBounds(); 
        
        var bounds = collisionTilemap.cellBounds;
        var mapSize = new int2(bounds.size.x, bounds.size.y);
        var gridOrigin = new int2(bounds.min.x, bounds.min.y);
        
        var builder = new BlobBuilder(Allocator.Temp);
        ref var gridData = ref builder.ConstructRoot<PathfindingGridData>();
        var gridArray = builder.Allocate(ref gridData.Grid, mapSize.x * mapSize.y);

        int i = 0;
        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                gridArray[i] = MapManager.Instance.IsCollision(new Vector3Int(x, y, 0));
                i++;
            }
        }
        
        var gridBlob = builder.CreateBlobAssetReference<PathfindingGridData>(Allocator.Persistent);
        builder.Dispose();

        var singletonEntity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(singletonEntity, new PathfindingParams
        {
            MapSize = mapSize,
            GridOrigin = gridOrigin, 
            GridBlob = gridBlob
        });

        state.EntityManager.CreateEntity(typeof(MapConvertedTag));
        Debug.Log($"[MapConversionSystem] 맵 변환 완료.");
    }
}

