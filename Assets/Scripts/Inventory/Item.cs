using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    
    public virtual void Use()
    {
        // 아이템 사용 효과
        Debug.Log("Using " + itemName);
    }
}
