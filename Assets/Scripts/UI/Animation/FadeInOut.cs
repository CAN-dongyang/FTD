using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FadeInOut : UIGroup
{
	public float time = 2f;
	public UnityEvent OnCompleted = new();

	private bool _toZero;
	private float Sign => _toZero ? -1f : 1f;
	private bool IsRunning => _toZero ? _canvasGroup.alpha > 0f : _canvasGroup.alpha < 1f;

	public void FadeIn()
	{
		if(!IsRunning)
		{
			_toZero = true;
			StartCoroutine(FadeRoutine());
		}
		else _toZero = true;
	}
	public void FadeOut()
	{
		if(!IsRunning)
		{
			_toZero = false;
			StartCoroutine(FadeRoutine());
		}
		else _toZero = false;
	}

	IEnumerator FadeRoutine()
	{
		while(IsRunning)
		{
			_canvasGroup.alpha += Sign / time * Time.deltaTime;
			yield return null;
		}
		_canvasGroup.alpha = _toZero ? 0f : 1f;

		OnCompleted.Invoke();
	}
}