using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SchoolUIControl : MonoBehaviour
{
	public enum MENU
	{
		INFO,
		ACTIVITY,
		MEMBER,
		MANAGEMENT
	}
	[Serializable]
	public struct TabMenu { public MENU key; public UIGroup view; public Button button; }
	[SerializeField] private List<TabMenu> _menus;

	private Canvas _canvas;
	
	public MENU NowMenu { get; private set; } = MENU.INFO;
	public void SelectMenu(MENU key)
	{
		_menus.ForEach(m => m.view.ActivateUI(m.key == key));
		NowMenu = key;
	}

	private void ToggleUI(InputAction.CallbackContext ctx) { _canvas.enabled = !_canvas.enabled; }
	private void Cancel(InputAction.CallbackContext ctx) { _canvas.enabled = false; }
	private void OnEnable()
	{
		_canvas = GetComponent<Canvas>();
		_canvas.enabled = false;

		_menus.ForEach(m => m.button.onClick.AddListener(() => SelectMenu(m.key)));

		var action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started += Cancel;
	}
	private void OnDisable()
	{
		_menus.ForEach(m => m.button.onClick.RemoveAllListeners());

		var action = InputSystem.actions.FindAction("Cancel");
		if(action != null) action.started -= Cancel;
	}
}