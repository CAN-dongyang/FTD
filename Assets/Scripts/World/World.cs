using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
	[SerializeField] private WorldData _data;
	public static WorldData Data => Instance._data;
	// Time Event Lists (함수 자체를 저장)
	public List<Func<IEnumerable>> OnDayStart = new();
	public List<Func<IEnumerable>> OnDayEnd = new();
	public List<Func<IEnumerable>> OnQuaterStart = new();
	public List<Func<IEnumerable>> OnQuaterEnd = new();
	private WorldTime _time;

	public void Pause()
	{
		Player.Instance.InputHandled = false;
		_time.Pause();
	}
	public void Resume()
	{
		Player.Instance.InputHandled = true;
		_time.Resume();
	}

	private void FixedUpdate()
	{
		if(!WorldTime.IsPaused) _time.Tick(Time.fixedDeltaTime);
	}
	private void Start() => StartCoroutine(GameFlow());

	#region Routines
	private IEnumerator GameFlow()
	{
		if(_time == null) // 게임을 막 시작했을 경우
		{
			yield return StartCoroutine(InitRoutine()); // Init 루틴 실행하고 기다림
		}

		while(Application.isPlaying)
		{
			InputSystem.actions.Enable();

			yield return RunEventList(OnDayStart);
			Debug.Log($"World : Day Started. d:{WorldTime.Day}");

			while(!WorldTime.IsEnded) // 하루가 끝나기 전까지
				yield return null;

			yield return RunEventList(OnDayEnd);
			Debug.Log($"World : Day Ended. d:{WorldTime.Day}");

			_time.MoveNextDay();
			// if quater ?

			InputSystem.actions.Disable();
		}
	}
	
	// SaveData를 불러와 세팅한다
	IEnumerator InitRoutine()
	{
		_time = WorldTime.Init(); // 자동 pause 상태

		if(SaveData.Exists())
		{
			var sav = SaveData.Load();

			_time._year = sav.Year;
			_time._quater = sav.Quater;
			_time._day = sav.Day;
		}

		while(Player.Instance == null) // Player 기다림
			yield return null;
		
		SceneManager.LoadScene("Scenes/School", LoadSceneMode.Additive);
		while(School.Instance == null) // School 기다림
			yield return null;
	}

	private IEnumerator RunEventList(List<Func<IEnumerable>> eventList)
	{
		if(eventList == null || eventList.Count == 0) yield break;

		var safeList = new List<Func<IEnumerable>>(eventList);
		foreach(var factory in safeList)
		{
			if(factory != null)
			{
				// 등록된 함수를 실행해 '새 Enumerable'을 생성
				IEnumerable routine = factory.Invoke();

				// 해당 루틴 실행 - 끝날 때까지 대기
				foreach(var step in routine)
				{
					// 상위 코루틴에게도 대기를 알림
					yield return step;
				}
			}
		}
	}
	#endregion

	#region Singleton
	private static World _instance = null;
	public static World Instance
	{
		get
		{
			if(!_instance) _instance = FindAnyObjectByType<World>();
			return _instance;
		}
	}
	#endregion
}

/* World : Singleton. n 개의 GameScene을 가진다

각 GameScene의 구성
	Data
	UI_Control
		UIGroup[] ... (with CanvasGroup)

	Coroutine (***IEnumerable)
		OnDayStart
		OnDayEnd

		+ OnQuater[]
		// [] : Start / End

World Rule
1. World는 반드시 Player (Singleton GameScene)이 동작한다
2. World는 WorldTime을 관리한다.
3. World에 GameScene이 UI를 등록(reg)했다면
	- Input은 동작하지 않는다.
	- WorldTime은 Pause 상태이다.
	- 타 GameScene의 UI가 등록될 수 없다.

GameScene Rule

1. GameScene은 하나의 UnityEngine.Scene와 동급
2. WorldTime이 Pause일때를 존중한다
3. Input과 UI는 다른 Scene이 UI를 사용중일때 동작하지 않는다
	- World에서 사용중인지 확인을 요한다 (mutex적 접근)

*/