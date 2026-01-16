using UnityEngine;

[CreateAssetMenu(fileName="PlayerData")]
public class PlayerData : ScriptableObject
{	
	public int money = 1000; // 초기 자금
	public void Initialize()
	{
	}
	public void Release()
	{
	}
}