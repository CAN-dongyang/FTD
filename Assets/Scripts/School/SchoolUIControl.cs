using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SchoolUIControl : MonoBehaviour
{
	public enum MENU
	{
		NONE,
		INFO,
		ACTIVITY,
		MEMBER,
		EMPLOYMENT
	}
	[Serializable]
	public struct TabMenu { public MENU key; public UIGroup view; public Button tabButton; }
	[SerializeField] private List<TabMenu> _menus;

	private Canvas _canvas;
	
	public MENU NowMenu { get; private set; } = MENU.NONE;
	public void OpenUI() => SelectMenu(MENU.INFO);
	public void SelectMenu(MENU key)
	{
		_menus.ForEach(m => m.view.ActivateUI(m.key == key));
		_canvas.enabled = key != MENU.NONE;

		// UI가 열릴 때 World 정지
		if(key != MENU.NONE) World.Instance.Pause();
		// UI가 열렸다가 닫힐 때 World 재개
		else if(NowMenu != MENU.NONE) World.Instance.Resume();

		NowMenu = key;
	}

	private void Cancel(InputAction.CallbackContext ctx)
	{
		if(NowMenu != MENU.NONE) SelectMenu(MENU.NONE);
	}
	private void OnEnable()
	{
		_canvas = GetComponent<Canvas>();
		_canvas.enabled = false;

		_menus.ForEach(m => m.tabButton.onClick.AddListener(() => SelectMenu(m.key)));

		var action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started += Cancel;
	}
	private void OnDisable()
	{
		_menus.ForEach(m => m.tabButton.onClick.RemoveAllListeners());

		var action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started -= Cancel;
	}
}