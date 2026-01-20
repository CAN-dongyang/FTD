using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIControl : MonoBehaviour
{
	[Flags]
	public enum MENU
	{
		NONE = 1,
		INVENTORY = 2 << 1,
		PLAYER_INFO = 2 << 2,
		SETTINGS = 2 << 3,
		TALK = 2 << 4
	}
	[Serializable]
	public struct TabMenu {  public UIGroup view; public MENU key; }
	[SerializeField] private List<TabMenu> _menus;

	[SerializeField] public MENU NowMenu = MENU.NONE;
	public void SelectMenu(MENU key)
	{
		_menus.ForEach(m => m.view.ActivateUI((m.key & key) > 0));
		NowMenu = key;

		if(NowMenu != MENU.NONE)
		{
			//GameData.Time.Pause();
			Player.Instance.InputHandled = false;
		}
		else
		{
			//GameData.Time.Resume();
			Player.Instance.InputHandled = true;
		}
	}

	private void OpenInventory(InputAction.CallbackContext ctx) => SelectMenu(MENU.INVENTORY);
	private void Cancel(InputAction.CallbackContext ctx) { if(NowMenu != MENU.NONE) SelectMenu(MENU.NONE); }
	private void Start() => SelectMenu(MENU.NONE);
	private void OnEnable()
	{
		var action = InputSystem.actions.FindAction("Inventory");
		if(action != null) action.started += OpenInventory;

		action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started += Cancel;
	}
	private void OnDisable()
	{
		var action = InputSystem.actions.FindAction("Inventory");
		if(action != null) action.started -= OpenInventory;

		action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started -= Cancel;
	}
}