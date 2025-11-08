using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
[UpdateAfter(typeof(DepartureSystem))] 
public partial struct StudentMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.GetSingleton<GameTime>().IsPaused) return;

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        float deltaTime = SystemAPI.Time.DeltaTime;

        // --- 여기가 수정되었습니다: CurrentScheduleIndex를 함께 조회합니다 ---
        foreach (var (transform, pathBuffer, pathIndex, speed, scheduleIndex, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, DynamicBuffer<PathWaypoint>, RefRW<CurrentPathIndex>, RefRO<MoveSpeedComponent>, RefRW<CurrentScheduleIndex>>()
                 .WithAll<IsMovingTag>()
                 .WithNone<PlayerTag>()
                 .WithEntityAccess())
        {
            if (pathBuffer.Length == 0 || pathIndex.ValueRO.Value >= pathBuffer.Length)
            {
                ecb.RemoveComponent<IsMovingTag>(entity);
                ecb.RemoveComponent<PathWaypoint>(entity);
                ecb.SetComponent(entity, new CurrentPathIndex { Value = -1 });

                // --- 여기가 수정되었습니다: 도착 보고 및 다음 스케줄 준비 ---
                // 이동이 끝났으므로, 스케줄 인덱스를 1 증가시켜 다음 일정을 준비합니다.
                scheduleIndex.ValueRW.Value++;
                continue;
            }

            float3 targetPosition = pathBuffer[pathIndex.ValueRO.Value].Position;
            float3 position = transform.ValueRO.Position;
            
            float movementDistance = speed.ValueRO.Value * deltaTime;
            float distanceToTarget = math.distance(position, targetPosition);

            if (movementDistance >= distanceToTarget)
            {
                transform.ValueRW.Position = targetPosition;
                pathIndex.ValueRW.Value++;
            }
            else
            {
                transform.ValueRW.Position += math.normalize(targetPosition - position) * movementDistance;
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}