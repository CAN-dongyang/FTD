using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIControl : MonoBehaviour
{
	[Flags]
	public enum MENU
	{
		LOCK = 0,
		DEFAULT = 2 << 0,
		INVENTORY = 2 << 1,
		PLAYER_INFO = 2 << 2,
		SETTINGS = 2 << 3,
		TALK = 2 << 4
	}
	[Serializable]
	public struct TabMenu {  public UIGroup view; public MENU key; }
	[SerializeField] private List<TabMenu> _menus = new();

	[SerializeField] public MENU NowMenu = MENU.LOCK;
	public void SelectMenu(MENU key)
	{
		_menus.ForEach(m => m.view.ActivateUI((m.key & key) > 0));
		
		if(key > MENU.DEFAULT) World.Instance.Pause();
		else if(NowMenu > MENU.DEFAULT) World.Instance.Resume();
		
		NowMenu = key;
	}

	private void OpenInventory(InputAction.CallbackContext ctx)
	{
		if(NowMenu != MENU.LOCK)
		{
			// Toggle
			if(NowMenu == MENU.INVENTORY) SelectMenu(MENU.DEFAULT);
			else SelectMenu(MENU.INVENTORY);
		}
	}
	private void Cancel(InputAction.CallbackContext ctx)
	{
		if(NowMenu > MENU.DEFAULT) SelectMenu(MENU.DEFAULT);
	}
	private void Start() => SelectMenu(MENU.DEFAULT);
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