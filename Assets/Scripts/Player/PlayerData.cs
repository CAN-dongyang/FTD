using UnityEngine;

[CreateAssetMenu(fileName="PlayerData")]
public class PlayerData : ScriptableObject
{
    public int money = 1000; // 초기 소지금

	public void Initialize()
	{
	}
	public void Release()
	{
	}
}