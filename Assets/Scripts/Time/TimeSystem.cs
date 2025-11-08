using Unity.Entities;
using Unity.Burst;

// SystemGroup 혹은 Order의 수정을 통해 업데이트 최우선순위를 갖도록 한다
[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct TimeSystem : ISystem
{
	// OnCreate는 시스템이 처음 생성될 때 한번만 호출됩니다.
	public void OnCreate(ref SystemState state)
	{
		/*	Save Data에서 현재 시간 정보를 가져온 후 GameTime 컴포넌트에 복사 */

		// object saveFile;
		GameTime time = new()
		{
			Year = 1,
			Quater = 1,
			Day = 1,
			TimeOfDay = 360f,	// 06:30 am
			TimeScale = 1f
		};

		// GameTime 컴포넌트로 Singleton Entity를 만듭니다
		var timeEntity = state.EntityManager.CreateSingleton(time);

		// Entity Manager에 time Entity 추가
		state.EntityManager.AddComponent(timeEntity, typeof(GameTime));
	}

	public void OnUpdate(ref SystemState state)
	{
		// 유일한 GameTime를 찾아서 수정 가능한 상태(RW)로 가져옵니다.
		var gameTime = SystemAPI.GetSingletonRW<GameTime>();
		var gameTimeRO = gameTime.ValueRO;

		if (gameTimeRO.IsPaused) return;

		// 1. TimeScale 결정
		// - 밤이면 20배속, 아니면 10배속
		if (gameTimeRO.TimeOfDay >= 120f && gameTimeRO.TimeOfDay < 360f)
		{
			gameTime.ValueRW.TimeScale = 20.0f; // 20배속
		}
		else
		{
			gameTime.ValueRW.TimeScale = 10.0f; // 10배속
		}

		// 2. 시간 흐름 진행
		gameTime.ValueRW.TimeOfDay += SystemAPI.Time.DeltaTime * gameTime.ValueRW.TimeScale;

		// 3. 시간 이벤트 실행
		// - Day End
		if (gameTimeRO.TimeOfDay > GameTime.MinutesOfDay)
		{
			gameTime.ValueRW.Day++;
			gameTime.ValueRW.TimeOfDay -= GameTime.MinutesOfDay;

			// Day End Events
#if UNITY_EDITOR
			UnityEngine.Debug.LogFormat("Day End");
#endif
		}
		// - Quater End
		if (gameTimeRO.Day >= GameTime.DaysOfQuater)
		{
			gameTime.ValueRW.Quater++;
			gameTime.ValueRW.Day = 1;

			// Quater End Events
#if UNITY_EDITOR
			UnityEngine.Debug.LogFormat("Quater End");
#endif
			// - ScheduleUpdate System 활성화
		}
		// - Year End
		if (gameTimeRO.Quater >= GameTime.QuatersOfYear)
		{
			gameTime.ValueRW.Year++;
			gameTime.ValueRW.Quater = 1;

			// Year End Events
#if UNITY_EDITOR
			UnityEngine.Debug.LogFormat("Year End");
#endif
		}
	}
}