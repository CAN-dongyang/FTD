using UnityEngine;

[CreateAssetMenu(fileName="GameData")]
public class GameData : ScriptableObject
{
	public GameTime time;
	public static GameTime Time => Instance.time;

	#region Singleton
	private static GameData _instance;
	public static GameData Instance
	{
		get
		{
			if(_instance == null) _instance = Resources.Load<GameData>("GameData");
			return _instance;
		}
	}
	public void Initialize()
	{
	}
	public void Release()
	{
		if(_instance)
		{
			Resources.UnloadAsset(_instance);
			_instance = null;
		}
	}
	#endregion
}