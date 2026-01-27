using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ConstructionManager : MonoBehaviour
{
	[SerializeField] private UIGroup _ui;
	[SerializeField] private CinemachineCamera _camera;

	[Header("Preview")]
	[SerializeField] private TilemapRenderer _gridOverlay;
	[SerializeField] private Tilemap _previewMap;
	private Vector2 _mousePos;

	private void FixedUpdate()
	{
		if(_ui.IsShow)
		{
			var worldPos = Camera.main.ScreenToWorldPoint(_mousePos);
			_gridOverlay.transform.position =
				School.GridInternal.WorldToCell(worldPos);
		}
	}

	private void GetMousePos(InputAction.CallbackContext ctx)
	{
		_mousePos = ctx.ReadValue<Vector2>();
	}
	public void Open()
	{
		Player.Instance.InputHandled = false;

		var act = InputSystem.actions.FindAction("Cancel");
		act.started += Cancel;

		act = InputSystem.actions.FindAction("Point");
		act.performed += GetMousePos;
		_mousePos = act.ReadValue<Vector2>();

		_gridOverlay.enabled = true;

		_camera.enabled = true;
		_ui.OpenUI();
	}
	public void Close()
	{
		var act = InputSystem.actions.FindAction("Cancel");
		act.started -= Cancel;

		act = InputSystem.actions.FindAction("Point");
		act.performed -= GetMousePos;

		_ui.CloseUI();
		_camera.enabled = false;

		_gridOverlay.enabled = false;
		_previewMap.SetTile(Vector3Int.zero, null);

		Player.Instance.InputHandled = true;
	}
	private void Start() => Close();


	private void Cancel(InputAction.CallbackContext ctx) => Close();
}