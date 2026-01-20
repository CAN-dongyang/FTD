using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIGroup : MonoBehaviour
{
	public void OpenUI() => IsShow = true;
	public void CloseUI() => IsShow = false;
	public void ActivateUI(bool active) => IsShow = active;
	public void ToggleUI() => IsShow = !IsShow;

	public bool IsShow
	{
		get => _canvasGroup.alpha > 0f;
		private set { if(IsShow != value) SetActivateUI(value); }
	}
	// 상속 용도
	protected virtual void SetActivateUI(bool active)
	{
		_canvasGroup.alpha = active ? 1f : 0f;
		_canvasGroup.interactable = _canvasGroup.blocksRaycasts = active;
	}

	protected virtual void Awake() => _canvasGroup = GetComponent<CanvasGroup>();
	protected CanvasGroup _canvasGroup;
}