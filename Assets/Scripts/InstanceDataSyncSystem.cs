using Unity.Entities;
using Unity.Collections;
using System;

// Managed 데이터 접근을 위해 SystemBase 사용.		<< DOTS or Managed(Unity Main Thread)
// 이 시스템 또한 메인 스레드에서 실행
public partial class InstanceDataSyncSystem : SystemBase
{
	protected override void OnUpdate()
	{
		try
		{
			var instanceDataManager = InstanceDataManager.Instance;
		}
		catch (NullReferenceException e)
		{
			UnityEngine.Debug.LogError($"{e.Message} {this}를 종료합니다");

			Enabled = false;
			return;
		}

		var ecb = new EntityCommandBuffer(Allocator.Temp);
		
		// MainThread이므로 .Run() 대신 .ForEach 사용
		Entities.ForEach((Entity entity, in UpdateDataRequest tag) =>
		{
			int instanceID = tag.InstanceID;

			// 1. InstanceID로 RuntimeData 찾기

			// 2. 매니지드 객체의 메서드로 Sync

			// 3. 처리 완료된 태그 제거
			ecb.RemoveComponent<UpdateDataRequest>(entity);
		}).WithoutBurst().Run();

		ecb.Playback(EntityManager);
	}
}