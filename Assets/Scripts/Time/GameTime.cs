using System;
using UnityEngine;

[Serializable]
// 게임의 전역 시간 정보 (Singleton Entity에 존재)
public class GameTime
{
	public int Year;			// 현재 년도 [ n ~ inf ]
	public int Quater;			// 현재 분기 [ 1 ~ 4 ]
	public int Day;				// 현재 일
	[SerializeField] private float _timeOfDay = 0f;
	public float TimeOfDay => _timeOfDay;		// 현재 시간 (min) [ 0.0 ~ 1440.0 ]
	[SerializeField] private float _timeScale = 1f;
	public float TimeScale => _timeScale;     // 시간 흐름 속도 (0.0 == 정지)

	public void Tick(float deltaTime)
	{
		if(!IsPaused) _timeOfDay += deltaTime * TimeScale;
	}
	/// 기본 : 오전 6시 (360)
	public void Reset(int timeOfDay = 360) => _timeOfDay = timeOfDay;
	public void Resume(float scale=1f) => _timeScale = scale;
	public void Pause() => _timeScale = 0f;

	// ----- ----- Properties ----- -----
	public bool IsPaused => TimeScale.Equals(0f);
	public DayOfWeek DayOfWeek => (DayOfWeek)((Day - 1) % 7);
	public int Hour => (int)(TimeOfDay / 60);
	public int Minutes => (int)(TimeOfDay % 60);

	// ----- ----- Constants ----- -----
	public const float MinutesOfDay = 1440f;        // 하루의 max time 값
	public const int DaysOfQuater = 91;             // 쿼터의 max day 값 ( 91.25f )
	public const int QuatersOfYear = 4;				// 1년의 max quater 값
}