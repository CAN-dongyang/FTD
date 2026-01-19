using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : ScriptableObject
{
	public PlayerStatus Status;
	public PlayerInventory Inventory;
	public int money = 1000; // 초기 자금
	public int silver;


	public void Initialize()
	{
	}
	public void Release()
	{
	}
}