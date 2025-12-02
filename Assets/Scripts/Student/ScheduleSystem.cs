using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct ScheduleSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PathfindingParams>();
    }

    [BurstDiscard]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.GetSingleton<GameTime>().IsPaused) return;

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var gameTime = SystemAPI.GetSingleton<GameTime>();
        var today = (GameDayOfWeek)((gameTime.Day - 1) % 7);
        const float planningTimeWindow = 60f; // 60분 (1시간)

        // --- 여기가 수정되었습니다: WithNone을 체인 방식으로 올바르게 수정 ---
        // 이동과 관련된 어떤 상태에도 있지 않은, 완전히 '유휴' 상태인 학생만 찾습니다.
        foreach (var (scheduleIndex, scheduleBuffer, transform, entity) in
                 SystemAPI.Query<RefRW<CurrentScheduleIndex>, DynamicBuffer<ScheduleEntry>, RefRO<LocalTransform>>()
                 .WithNone<PathRequest>()
                 .WithNone<PendingMovement>()
                 .WithNone<IsMovingTag>()
                 .WithNone<PlayerTag>()
                 .WithEntityAccess())
		{
            int currentIndex = scheduleIndex.ValueRO.Value;
            
            while (currentIndex < scheduleBuffer.Length && scheduleBuffer[currentIndex].Day != today)
            {
                currentIndex++;
            }

            if (currentIndex < scheduleBuffer.Length && scheduleBuffer[currentIndex].Day == today)
            {
                var entry = scheduleBuffer[currentIndex];

                // 수업 시작 1시간 전부터 길찾기를 미리 준비합니다.
                if (gameTime.TimeOfDay >= (entry.StartTime - planningTimeWindow))
                {
                    if (math.all(entry.TargetPosition == float3.zero))
                    {
                        scheduleIndex.ValueRW.Value = currentIndex + 1;
                        continue; 
                    }

                    ecb.AddComponent(entity, new PathRequest
                    {
                        StartPosition = transform.ValueRO.Position,
                        EndPosition = entry.TargetPosition
                    });
                    ecb.AddComponent(entity, new PendingMovement { ScheduledStartTime = entry.StartTime });
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}