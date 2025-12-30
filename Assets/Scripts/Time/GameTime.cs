using System;
using Unity.Entities;

// 게임의 전역 시간 정보 (Singleton Entity에 존재)
public struct GameTime : IComponentData
{
	public int Year;			// 현재 년도 [ n ~ inf ]
	public int Quater;			// 현재 분기 [ 1 ~ 4 ]
	public int Day;				// 현재 일
	public float TimeOfDay;		// 현재 시간 (min) [ 0.0 ~ 1440.0 ]
	public float TimeScale;     // 시간 흐름 속도 (0.0 == 정지)

	// ----- ----- Properties ----- -----
	public bool IsPaused
	{
		get => TimeScale.Equals(0f);
		set => TimeScale = value ? 0f : 1f;     // 기본 Scale 값은 1이다
	}
	public DayOfWeek DayOfWeek => (DayOfWeek)((Day - 1) % 7);
	public int Hour => (int)(TimeOfDay / 60);
	public int Minutes => (int)(TimeOfDay % 60);

	// ----- ----- Constants ----- -----
	public const float MinutesOfDay = 1440f;        // 하루의 max time 값
	public const int DaysOfQuater = 91;             // 쿼터의 max day 값 ( 91.25f )
	public const int QuatersOfYear = 4;				// 1년의 max quater 값
}