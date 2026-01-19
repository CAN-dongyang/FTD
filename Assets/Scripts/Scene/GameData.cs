using UnityEngine;

public class GameData : ScriptableObject
{
	public GameTime time;
	public static GameTime Time => Instance.time;


	#region Singleton
	private static GameData Instance { get; set; }
	static public void Initialize()
	{
		if(!Instance)
		{
			Instance = Resources.Load<GameData>("GameData");
		}
	}
	static public void Release()
	{
		if(Instance)
		{
			Resources.UnloadAsset(Instance);
			Instance = null;
		}
	}
	#endregion
}