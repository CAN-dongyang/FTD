using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIView : MonoBehaviour
{
	[SerializeField] private UnityEvent _onEnabled;
	[SerializeField] private UnityEvent _onDisabled;

	public void OpenUI() => IsShow = true;
	public void CloseUI() => IsShow = false;
	public void ActivateUI(bool active) => IsShow = active;
	public void ToggleUI() => IsShow = !IsShow;

	public bool IsShow
	{
		get => _cg.alpha > 0f;
		private set { if(IsShow != value) SetActivateUI(value); }
	}
	protected virtual void SetActivateUI(bool active)
	{
		_cg.alpha = active ? 1f : 0f;
		_cg.interactable = _cg.blocksRaycasts = active;

		if(active) _onEnabled.Invoke();
		else _onDisabled.Invoke();
	}

	private void Awake()
	{
		_cg = GetComponent<CanvasGroup>();
		IsShow = false;
	}
	private CanvasGroup _cg;
}