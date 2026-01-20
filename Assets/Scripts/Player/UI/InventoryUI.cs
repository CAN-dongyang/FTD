using UnityEngine;

public class InventoryUI : UIGroup
{
	private HotbarSlot[] _hotbars;
	
	private void Start()
	{
		_hotbars = GetComponentsInChildren<HotbarSlot>();
	}
}