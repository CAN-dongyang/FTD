using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Singleton
/// >>>
/// 	Instance Data의 생성과 인덱싱 부여, 관리 |
/// 	Instance 데이터와 DOTS 데이터 사이의 파싱, 자동 동기화
/// </summary>
public class InstanceDataManager : MonoBehaviour, ISerializationCallbackReceiver
{
	// 쓰고 읽어올 jsons 파일의 이름
	[SerializeField] private string fileName = "InstanceDatas.json";
#if UNITY_EDITOR
	public string FilePath => $"{Application.dataPath}/Resources/Entity/{fileName}";
#else
	public string FilePath => Application.persistentDataPath + fileName;
#endif

	[SerializeReference] // base class가 아닌 inherit type class로 저장됨
	[SerializeField]
	private List<InstanceData> DataList = new(); // 저장과 수정
	private Dictionary<InstanceDataID, InstanceData> Datas = new(); // 검색 ( O(1) )

	public static List<InstanceData> GetAllData() => Instance.DataList;

	public static void AddData<DATA_TYPE>(DATA_TYPE data)
		where DATA_TYPE : InstanceData
	{
		if (Instance.Datas.ContainsKey(data.ID)) return;

		Instance.DataList.Add(data);
		Instance.SaveAll();
	}
	public static void RemoveDataID(InstanceDataID id)
	{
		if (Instance.Datas.Remove(id)) //지워졌다면
			Instance.SaveAll();
	}
	public static InstanceDataID GetNewInstanceID(int assetID, InstanceDataType dataType)
	{
		InstanceDataID newID = new(assetID, 0);
		
		// sub type 값을 침범하기 전까지
		for (int i = 1; i < IDStructure.indent_s; i++)
		{
			newID.instanceID = (int)dataType + i;
			if (!Instance.Datas.ContainsKey(newID)) break;
		}
		return newID;
	}

	public static EntityAssetType GetAssetType(int assetID)
	{
		int removeIndex = assetID - (assetID % IDStructure.indent_c);
		return (EntityAssetType)removeIndex;
	}
	public static InstanceDataType GetDataType(int instanceID)
	{
		int removeIndex = instanceID - (instanceID % IDStructure.indent_e);
		return (InstanceDataType)removeIndex;
	}

	public void LoadAll()
	{
		if (DataList != null) DataList.Clear();
		else DataList = new();

		if (!File.Exists(FilePath)) return;

		string jsons = File.ReadAllText(FilePath);
		foreach (var line in jsons.Split('\n')) // 한 줄씩 읽는다
		{
			if (!line.Contains('{')) continue; // json 형태가 없다면 읽지 않는다

			var split = line.IndexOf('{');
			var dataType = (InstanceDataType)int.Parse(line.Substring(0, split));
			var json = line.Substring(split, line.Length - split);

			switch (dataType)
			{
				case InstanceDataType.Lesson:
					DataList.Add(JsonUtility.FromJson<LessonInstanceData>(json));
					break;
				case InstanceDataType.Work:
					DataList.Add(JsonUtility.FromJson<WorkInstanceData>(json));
					break;
				case InstanceDataType.Student:
					DataList.Add(JsonUtility.FromJson<StudentInstanceData>(json));
					break;
				case InstanceDataType.Professor:
					DataList.Add(JsonUtility.FromJson<ProfessorInstanceData>(json));
					break;
				case InstanceDataType.Worker:
					DataList.Add(JsonUtility.FromJson<WorkerInstanceData>(json));
					break;
				case InstanceDataType.Organization:
					DataList.Add(JsonUtility.FromJson<OrganizationInstanceData>(json));
					break;
			}
		}
		Debug.Log($"Load InstanceData Jsons");
	}
	public void SaveAll()
	{
		string jsons = string.Empty;
		foreach (var d in Instance.DataList)
		{
			// 한 줄에 (dataType + data(json) + \n)의 형태로 저장
			jsons += $"{(int)d.DataType}{JsonUtility.ToJson(d)}\n";
		}
		File.WriteAllText(FilePath, jsons);

		Debug.Log($"Save InstanceData Jsons to {FilePath}");
	}

	#region DOTS Data Syncs

	#endregion

	#region Serialize
	public void OnBeforeSerialize()
	{
	}
	public void OnAfterDeserialize()
	{
		foreach (var data in DataList)
			if (data != null && data.ID.entityID != 0 && Datas.ContainsKey(data.ID))
				Datas.Add(data.ID, data);
	}
	#endregion

	#region Singleton
	private static InstanceDataManager _instance;
	public static InstanceDataManager Instance
	{
		get
		{
			if ((_instance ??= FindAnyObjectByType<InstanceDataManager>()) == null) // 인스턴스가 없다면
			{
				GameObject go = new GameObject("InstanceData Manager");
				_instance = go.AddComponent<InstanceDataManager>();
			}
			return _instance;
		}
	}
	private void OnDestroy()
	{
		if (Instance == this) _instance = null;
	}
	#endregion
}