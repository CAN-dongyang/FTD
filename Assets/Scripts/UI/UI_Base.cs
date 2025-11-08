using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UI_Base : MonoBehaviour
{
    protected virtual void Start()
    {
        // UI 초기화 관련 로직
    }

    // UI 창을 드래그 가능하게 하는 로직
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
