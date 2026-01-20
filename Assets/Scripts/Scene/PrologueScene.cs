using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PrologueScene : MonoBehaviour
{
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

		SaveData save;
		if(SaveData.Exists()) save = SaveData.Load();
		else save = new SaveData();
		
		save.GameProgress = 1;
		SaveData.Save(save);

		SceneManager.LoadScene("Scenes/MainGame");
	}
}