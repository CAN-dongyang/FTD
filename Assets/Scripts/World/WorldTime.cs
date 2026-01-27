using System;
using UnityEngine;

/// <summary>
/// 게임의 전역 시간 정보
/// </summary>
/// public static : 모두 전역 접근 (get)
/// public : Instance를 가진 World만 접근
public class WorldTime
{
	public int _year;			// 현재 년도 [ n ~ inf ]
	public int _quater;			// 현재 분기 [ 1 ~ 4 ]
	public int _day;			// 현재 일
	[SerializeField] private float _timeOfDay = 0f; // 현재 시간 (min) [ 0.0 ~ 1440.0 ]
	[SerializeField] private float _timeScale = 0f; // 시간 흐름 속도 (0.0 == 정지)

	public static int Year		=> Instance._year;
	public static int Quater	=> Instance._quater;
	public static int Day		=> Instance._day;
	public static float TimeOfDay		=> Instance._timeOfDay;
	public static float TimeScale		=> Instance._timeScale;

	// ----- ----- Properties ----- -----
	// Ceil(올림) : 0의 값인지 정확한 확인을 위해
	public static bool IsPaused			=> Mathf.CeilToInt(TimeScale) == 0;
	// Floor(내림) : Max 값인지 정확한 확인을 위해
	public static bool IsEnded			=> Mathf.FloorToInt(TimeOfDay) == MinutesOfDay;
	
	public static DayOfWeek DayOfWeek	=> (DayOfWeek)((Day - 1) % 7);
	public static int Hour				=> (int)(TimeOfDay / 60);
	public static int Minutes			=> (int)(TimeOfDay % 60);

	// ----- ----- Constants ----- -----
	public const int MinutesOfDay	= 1440;	// 하루의 max time 값
	public const int DaysOfQuater	= 91;	// 쿼터의 max day 값 ( 91.25f )
	public const int QuatersOfYear	= 4;	// 1년의 max quater 값

	// ----- ----- Init ----- -----
	public static WorldTime Init()
	{
		if(Instance == null) Instance = new();
		else Debug.LogWarning("WorldTime을 여러 번 할당 중입니다.");
		
		Instance.Reset();
		return Instance;
	}
	private static WorldTime Instance { get; set; }
	private WorldTime() // 생성자를 외부에서 사용 불가
	{
		// 종료 이벤트를 구독
		Application.quitting += Dispose;
	}
	private void Dispose()
	{
		// 종료 이벤트 구독 해제 (important)
		Application.quitting -= Dispose;

		// 종료 시 Instance 초기화
		Instance = null;
	}

	// ----- ----- Managed ----- -----
	public void Tick(float deltaTime)	=> _timeOfDay = Mathf.Clamp(_timeOfDay + deltaTime * TimeScale, 0f, MinutesOfDay);
	public void Close()					=> _timeOfDay = MinutesOfDay;
	public void Reset(int t = 360)		=> _timeOfDay = t; /// 기본 : 오전 6시 (360)
	public void Resume(float scale=1f)	=> _timeScale = scale;
	public void Pause()					=> _timeScale = 0f;

	public void MoveNextDay()
	{
		// 체크 사항 여러 개 ㅇㅇ
		_day++;
		Reset();
	}
}