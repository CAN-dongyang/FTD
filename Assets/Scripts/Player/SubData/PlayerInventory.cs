using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerInventory
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 40; // (핫바 10칸 + 인벤 30칸)
    public List<Item> items = new();

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room.");
                return false;
            }
            items.Add(item);
            onItemChangedCallback?.Invoke();
        }
        return true;
    }
    public void Remove(Item item)
    {
        items.Remove(item);
        onItemChangedCallback?.Invoke();
    }

    public void SortItems()
    {
        if(items.Count <= 10) return;

        List<Item> hotbarItems = items.GetRange(0, 10);
        List<Item> inventoryItems = items.GetRange(10, items.Count - 10);

        inventoryItems.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        items.Clear();
        items.AddRange(hotbarItems);
        items.AddRange(inventoryItems);

        if(onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
