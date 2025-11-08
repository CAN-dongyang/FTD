using Unity.Entities;
using Unity.Burst;

public partial struct DecisionMakingSystem : ISystem
{
	public void OnUpdate(ref SystemState state)
	{
		var gametIme = SystemAPI.GetSingleton<GameTime>();

		// ECB 가져오기 (엔티티 변경 작업을 병렬 처리에서도 안전하게 수행하기 위해)
		var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
		
		// CurrentAction이 없는 Member 엔티티에 대해 병렬로 작업 수행

		// 현재 시간에 맞는 스케줄 찾기

		// 개인 행동이 있는지 확인하여 스케줄과 경중 비교

		ecb.Playback(state.EntityManager);
		ecb.Dispose();
	}
}