using System.Collections;
using UnityEngine;

public class SceneController : MonoBehaviour
{
	[SerializeField] private GameData _gameData;

	public void Start()
	{
		_gameData.time.TimeScale = 1f;

		_gameData.time.TimeOfDay = 0f;
	}
	public void Pause()
	{
		_gameData.time.TimeScale = 0f;
	}
	
	private void Update()
	{
		if(!_gameData.time.IsPaused)
			_gameData.time.TimeOfDay += Time.deltaTime * _gameData.time.TimeScale;
	}
	private void Awake() => StartCoroutine(InitializeRoutine());
	private void OnDestroy()
	{
		_gameData.Release();
	}

	private IEnumerator InitializeRoutine()
	{
		// Initialize에 Resource Load등이 포함되면 async / await을 활용하기 때문에
		// 코루틴에서 모든 것이 init 될때까지 기다리고 시작하는 게 일반적
		
		Pause();
		_gameData.Initialize();
		
		while(GameData.Instance == null)
			yield return null;
		
		while(SchoolData.Instance == null)
			yield return null;
		
		Start();
	}
}