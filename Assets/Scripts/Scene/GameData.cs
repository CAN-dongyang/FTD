using UnityEngine;

public class GameData : ScriptableObject
{
	[SerializeField] private GameTime _time;
	public static GameTime Time => Instance._time;
	[SerializeField] private SaveData _save;
	public static SaveData Save => Instance._save;

	#region Singleton
	private static GameData Instance { get; set; }
	static public void Initialize()
	{
		if(!Instance)
		{
			Instance = Resources.Load<GameData>("GameData");
		}
		Instance._save = SaveData.Load();

		Time.Year = Save.Year;
		Time.Quater = Save.Quater;
		Time.Day = Save.Day;
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