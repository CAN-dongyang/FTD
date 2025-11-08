using Unity.Entities;
using Unity.Burst;

/*
	public void OnUpdate(ref SystemState state)
	{
		if (SystemAPI.GetSingleton<GameTime>().IsPaused) return;

		var ecb = new EntityCommandBuffer(Allocator.Temp);
		var gameTime = SystemAPI.GetSingleton<GameTime>();

		// 현재 게임의 'Day' 숫자를 7로 나눈 나머지로 요일을 계산합니다. (0=월, 1=화, ...)
		// Day가 1부터 시작하므로 (Day - 1)을 해줍니다.
		var today = (GameDayOfWeek)((gameTime.Day - 1) % 7);

		// CurrentScheduleIndex와 ScheduleEntry 버퍼를 가진 모든 엔티티를 찾습니다.
		foreach (var (index, scheduleBuffer, entity) in
				 SystemAPI.Query<RefRW<CurrentScheduleIndex>, DynamicBuffer<ScheduleEntry>>()
				 .WithNone<MoveToTargetComponent, PlayerTag>() // 플레이어는 제외
				 .WithEntityAccess())
		{
			int currentIndex = index.ValueRO.Value;

			// 현재 인덱스가 전체 시간표 길이를 벗어나면 더 이상 할 일이 없습니다.
			if (currentIndex >= scheduleBuffer.Length) continue;

			// 현재 인덱스의 일정이 오늘 것이 아니라면, 오늘 일정이 나올 때까지 인덱스를 건너뜁니다.
			while (scheduleBuffer[currentIndex].Day != today)
			{
				currentIndex++;
				// 만약 오늘 일정을 못 찾고 시간표 끝까지 도달하면 중단합니다.
				if (currentIndex >= scheduleBuffer.Length) break;
			}

			// 유효한 '오늘'의 일정을 찾았고, 아직 이동 명령을 받지 않았다면
			if (currentIndex < scheduleBuffer.Length && scheduleBuffer[currentIndex].Day == today)
			{
				var nextEntry = scheduleBuffer[currentIndex];
				// 현재 시간이 일정 시작 시간이 되었다면 행동을 지시합니다.
				if (gameTime.TimeOfDay >= nextEntry.StartTime)
				{
					ecb.AddComponent(entity, new MoveToTargetComponent
					{
						TargetPosition = nextEntry.TargetPosition
					});

					// 다음 일정을 기다리도록 인덱스를 1 증가시킵니다.
					index.ValueRW.Value = currentIndex + 1;
				}
			}
			else // 오늘 일정을 모두 마쳤거나, 오늘 일정이 없다면
			{
				// 다음 날을 위해 인덱스를 현재 위치로 고정합니다.
				index.ValueRW.Value = currentIndex;
			}
		}

		ecb.Playback(state.EntityManager);
		ecb.Dispose();
	}
*/