using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIControl : MonoBehaviour
{
	public enum MENU
	{
		NONE,
		INVENTORY,
		PLAYER_INFO,
		SETTINGS,
		TALK
	}
	[Serializable]
	public struct TabMenu { public MENU key; public UIView view; }
	[SerializeField] private List<TabMenu> _menus;

	public MENU NowMenu { get; private set; } = MENU.NONE;
	public void SelectMenu(MENU key)
	{
		_menus.ForEach(m => m.view.ActivateUI(m.key == key));
		NowMenu = key;

		if(NowMenu != MENU.NONE)
		{
			GameData.Time.Pause();
			Player.Instance.InputHandled = false;
		}
		else
		{
			GameData.Time.Start();
			Player.Instance.InputHandled = true;
		}
	}

	private void Start() => SelectMenu(MENU.NONE);
	private void OnEnable()
	{
		var action = InputSystem.actions.FindAction("Inventory");
		if(action != null) action.started += ctx => SelectMenu(MENU.INVENTORY);

		action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started += ctx => { if(NowMenu != MENU.NONE) SelectMenu(MENU.NONE); };
	}
	private void OnDisable()
	{
		var action = InputSystem.actions.FindAction("Inventory");
		if(action != null) action.started -= ctx => SelectMenu(MENU.INVENTORY);

		action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started -= ctx => { if(NowMenu != MENU.NONE) SelectMenu(MENU.NONE); };
	}
}