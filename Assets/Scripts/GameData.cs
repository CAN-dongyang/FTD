using UnityEngine;

[CreateAssetMenu(fileName="GameData")]
public class GameData : ScriptableObject
{
	public GameTime time;
	public static GameTime Time => Instance.time;

	public static GameData Instance { get; private set; }
	public void Initialize()
	{
		Instance = this;
	}
	public void Release()
	{
		if(Instance == this) Instance = null;
	}
}