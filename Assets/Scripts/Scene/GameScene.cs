using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
	public FadeInOut _fadeAnim;

	private void FixedUpdate()
	{
		if(!GameData.Time.IsPaused) GameData.Time.Tick(Time.fixedDeltaTime);
	}

	// Player
	IEnumerator DayStartRoutine()
	{
		_fadeAnim.OpenUI();

		GameData.Time.Reset();
		GameData.Time.Pause();

		InputSystem.actions.Disable();
		
		while(Player.Instance == null)
			yield return null;
		
		SceneManager.LoadScene("Scenes/School", LoadSceneMode.Additive);
		
		while(School.Instance == null)
			yield return null;
		
		// Home 에서 시작. 아직 시간 정지
		// Home Out 시에 DayRoutine 시작
		School.Instance.Home.MoveIn();

		_fadeAnim.OnCompleted.AddListener(() => 
		{
			InputSystem.actions.Enable();
			StartCoroutine(DayRoutine());
		});
		_fadeAnim.FadeIn();
	}
	IEnumerator DayRoutine()
	{
		GameData.Time.Resume();

		yield return null;
	}

	private void Awake()
	{
		GameData.Initialize();
		StartCoroutine(DayStartRoutine());
	}
	private void OnDestroy()
	{
		GameData.Release();
		InputSystem.actions.Disable();
	}
}