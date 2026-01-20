using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScene : MonoBehaviour
{
	public Button _btnStartGame;

	public void StartGame()
	{
		Destroy(EventSystem.current.gameObject);

		if(SaveData.Exists() && SaveData.Load().GameProgress > 0)
			SceneManager.LoadScene("Scenes/MainGame");
		else
			SceneManager.LoadScene("Scenes/Prologue");
	}
	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void Start()
	{
		SaveData.Save(new SaveData());

		_btnStartGame.GetComponentInChildren<TextMeshProUGUI>().text
			= SaveData.Exists() ? "Start Game" : "New Game";
	}
}