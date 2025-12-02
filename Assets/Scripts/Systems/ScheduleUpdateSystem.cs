using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;

// 분기 변경 시 다음 분기 스케줄을 현재 스케줄로 복사하는 시스템
// (분기 시작 시 한 번 실행)
[BurstCompile]
public partial struct ScheduleUpdateSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
	}

	public void OnUpdate(ref SystemState state)
	{
		if (SystemAPI.GetSingleton<GameTime>().IsPaused) return;

		float time = SystemAPI.GetSingleton<GameTime>().TimeOfDay;

		var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (ch, awake, entity) in SystemAPI.Query<RefRW<CharacterComponentData>, RefRW<IsAwakeTag>>()
			.WithEntityAccess())
		{
			ecb.RemoveComponent<IsAwakeTag>(entity);
		}
		foreach (var (org, awake, entity) in SystemAPI.Query<RefRW<OrganizationComponentData>, RefRW<IsAwakeTag>>()
			.WithEntityAccess())
        {
            ecb.RemoveComponent<IsAwakeTag>(entity);
        }
        ecb.Playback(state.EntityManager);

		// 모든 Organization 엔티티를 찾음

		// 해당 엔티티의 NextQuarterSchedule 버퍼를 CurrentQuarterSchedule 버퍼로 복사

		// ECB 생성 (엔티티 변경 작업을 병렬 처리에서도 안전하게 수행하기 위해)

		// NextquarterSchedule 버퍼 초기화
	}
}