using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SchoolUIControl : UIView
{
	public enum MENU
	{
		INFO,
		ACTIVITY,
		MEMBER,
		MANAGEMENT
	}
	[Serializable]
	public struct TabMenu { public MENU key; public UIView view; public Button button; }
	[SerializeField] private List<TabMenu> _menus;
	
	public MENU NowMenu { get; private set; } = MENU.INFO;
	public void SelectMenu(MENU key)
	{
		_menus.ForEach(m => m.view.ActivateUI(m.key == key));
		NowMenu = key;
	}

	protected override void SetActivateUI(bool active)
	{
		base.SetActivateUI(active);
		if(active) SelectMenu(NowMenu);
		Player.Instance.InputHandled = !active;
	}
	private void ToggleMenu(InputAction.CallbackContext ctx) => ToggleUI();
	private void OnEnable()
	{
		InputSystem.actions.FindAction("Debug1").started += ToggleMenu;
		_menus.ForEach(m => m.button.onClick.AddListener(() => SelectMenu(m.key)));

		var action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started += ctx => { if(IsShow) SetActivateUI(false); };
	}
	private void OnDisable()
	{
		InputSystem.actions.FindAction("Debug1").started -= ToggleMenu;
		_menus.ForEach(m => m.button.onClick.RemoveAllListeners());

		var action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started -= ctx => { if(IsShow) SetActivateUI(false); };
	}
}