
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
	public Image border; // 선택 상태를 표시할 테두리 이미지
	public Image icon;

    private Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SetBorderColor(Color color)
    {
        if (border != null)
        {
            border.color = color;
            border.enabled = true;
        }
    }

    public void HideBorder()
    {
        if (border != null)
        {
            border.enabled = false;
        }
    }
}
