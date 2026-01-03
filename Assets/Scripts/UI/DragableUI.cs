using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableUI : MonoBehaviour
{
	[SerializeField] private UnityEvent<GameObject> OnDragStart;
	[SerializeField] private UnityEvent<GameObject> OnDragEnd;

	public void OnDrag(PointerEventData e)
	{
		transform.position = e.position;
	}
}