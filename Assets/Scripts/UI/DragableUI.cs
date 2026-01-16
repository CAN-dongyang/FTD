using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public UnityEvent<Vector2> OnDragStart { get; private set; } = new();
	public UnityEvent<Vector2> OnDragPerform { get; private set; } = new();
	public UnityEvent<Vector2> OnDragEnd { get; private set; } = new();

	public void OnBeginDrag(PointerEventData eventData)
	{
		OnDragStart.Invoke(transform.position);
	}
	public void OnDrag(PointerEventData e)
	{
		transform.position = e.position;
		OnDragPerform.Invoke(transform.position);
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		OnDragEnd.Invoke(transform.position);
	}
}