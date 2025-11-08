using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PathfindingSystem))]
public partial struct DepartureSystem : ISystem
{
    private const float ActivityTimeScale = 10.0f;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.GetSingleton<GameTime>().IsPaused) return;

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var gameTime = SystemAPI.GetSingleton<GameTime>();

        // 경로 계산은 끝났지만, 아직 이동 중이 아닌 학생들을 찾습니다.
        foreach (var (pathBuffer, pendingMovement, speed, entity) in
                 SystemAPI.Query<DynamicBuffer<PathWaypoint>, RefRO<PendingMovement>, RefRO<MoveSpeedComponent>>()
                 .WithNone<IsMovingTag>()
                 .WithEntityAccess())
        {
            float estimatedTravelTimeInSeconds = pathBuffer.Length / speed.ValueRO.Value;
            float estimatedTravelTimeInGameMinutes = estimatedTravelTimeInSeconds * ActivityTimeScale;
            float departureTime = pendingMovement.ValueRO.ScheduledStartTime - estimatedTravelTimeInGameMinutes;

            if (gameTime.TimeOfDay >= departureTime)
            {
                // 출발 시간이 되면 IsMovingTag를 추가하여 '출발 신호'를 보냅니다.
                ecb.AddComponent<IsMovingTag>(entity);
                ecb.RemoveComponent<PendingMovement>(entity);
            }
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
