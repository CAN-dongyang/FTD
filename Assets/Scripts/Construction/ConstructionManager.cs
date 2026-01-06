using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class ConstructionManager : MonoBehaviour
{
	[SerializeField] private UIView _ui;
	[SerializeField] private DragableUI _dragItem;
	[SerializeField] private CinemachineCamera _camera;

	private Grid _grid;

	private void Pospos(Vector2 screenPos)
	{
		var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
		Debug.Log($"{screenPos} => {worldPos} => {_grid.WorldToCell(worldPos)}");
	}
	public void Open()
	{
		Player.Instance.InputHandled = false;

		var act = InputSystem.actions.FindAction("Cancel");
		act.started += Cancel;

		_grid = School.Instance.Grid;

		_dragItem.OnDragStart.AddListener(Pospos);
		_dragItem.OnDragEnd.AddListener(Pospos);

		_camera.gameObject.SetActive(true);
		_ui.OpenUI();
	}
	public void Close()
	{
		var act = InputSystem.actions.FindAction("Cancel");
		act.started -= Cancel;

		_ui.CloseUI();
		_camera.gameObject.SetActive(false);

		_dragItem.OnDragStart.RemoveListener(Pospos);
		_dragItem.OnDragEnd.RemoveListener(Pospos);

		Player.Instance.InputHandled = true;
	}
	private void Start() => Close();


	private void Cancel(InputAction.CallbackContext ctx) => Close();
}