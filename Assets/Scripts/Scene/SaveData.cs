using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
	public int GameProgress = 0;

	public int Year = 1200;
	public int Quater = 1;
	public int Day = 1;

    public List<int> HiredProfessorIds; // 고용된 교수의 ID 리스트

	private static string GetSavePath(string fileName)
	{
#if ANDROID
#else
		return Path.Combine(Application.dataPath, fileName + ".json");
#endif
	}
	public static bool Exists() => File.Exists(GetSavePath("save"));
	public static SaveData Load()
	{
		string path = GetSavePath("save");

		if(File.Exists(path))
		{
			string json = File.ReadAllText(path);
			return JsonUtility.FromJson<SaveData>(json);
		}
		Debug.Log("Can't Found Save File");
		return null;
	}
	public static void Save<T>(T data)
	{
		string path = GetSavePath("save");
		string json = JsonUtility.ToJson(data, true);

		File.WriteAllText(path, json);
		Debug.Log($"Save Complete : {path}");
	}
}