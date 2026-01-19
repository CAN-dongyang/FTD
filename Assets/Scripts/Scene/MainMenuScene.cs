using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScene : MonoBehaviour
{
	public Button _btnStartGame;
	public Button _btnLoadGame;

	private bool _interactable;
	private bool _saveExists;

	public void StartGame()
	{
		if(!_interactable) return;

		_interactable = false;
		if(_saveExists)
		{
		}
		else
		{
			SceneManager.LoadScene("Scenes/Prologue");
		}
	}
	public void LoadGame()
	{
		if(!_interactable) return;

		_interactable = false;
	}
	public void Quit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	/// ----- Test -----
	public void TestPrologueSkip()
	{
		StartCoroutine(FadeOut());
	}
	IEnumerator FadeOut()
	{
		var fade = FindAnyObjectByType<CanvasGroup>();
		float time = 3f;

		Destroy(EventSystem.current.gameObject);		
		while(fade.alpha < 1f)
		{
			fade.alpha += (time / 1f) * Time.deltaTime;
			yield return null;
		}
		SceneManager.LoadScene("Scenes/School");
	}

	private void Start() => StartCoroutine(Initialize());
	private IEnumerator Initialize()
	{
		_interactable = _saveExists = false;

		yield return null;

		if(_btnStartGame)
		_btnStartGame.GetComponentInChildren<TextMeshProUGUI>().text
			= _saveExists ? "Start Game" : "New Game";
		
		if(_btnLoadGame)
		_btnLoadGame.interactable = _saveExists;

		_interactable = true;
	}
}