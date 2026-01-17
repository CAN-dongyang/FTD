using UnityEngine;

[CreateAssetMenu(fileName="PlayerData")]
public class PlayerData : ScriptableObject
{
	public PlayerInventory Inventory;
	public int money = 1000; // 초기 자금


	public void Initialize()
	{
	}
	public void Release()
	{
	}
}