using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
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
	public string FilePath => $"{Application.dataPath}/Resources/DataAsset/{fileName}";
#else
	public string FilePath => Application.persistentDataPath + fileName;
#endif

	[SerializeReference] // base class가 아닌 inherit type class로 저장됨
	[SerializeField]
	private List<InstanceData> DataList = new(); // 저장과 수정
	private Dictionary<DataID, InstanceData> Datas = new(); // 검색 ( O(1) )

	private Dictionary<DataID, DataAsset> Assets = new();

	public static bool Contains(DataID id) => Instance.Datas.ContainsKey(id);
	
	#region Gets
	public static List<InstanceData> GetAllData() => Instance.DataList;
	public static InstanceData GetData(DataID id)
	{
		return Instance.Datas.TryGetValue(id, out InstanceData value) ? value : null;
	}
	public static T GetData<T>(DataID id) where T : InstanceData
	{
		return Instance.Datas.TryGetValue(id, out InstanceData value) ? value as T : null;
	}

	public static DataAsset GetAsset(DataID id)
	{
		return Instance.Assets.TryGetValue(id, out DataAsset value) ? value : null;
	}
	public static T GetAsset<T>(DataID id) where T : DataAsset
	{
		return Instance.Assets.TryGetValue(id, out DataAsset value) ? value as T : null;
	}
	#endregion

	#region Add / Remove
	public static bool AddData<DATA_TYPE>(DATA_TYPE data) where DATA_TYPE : InstanceData
	{
		if (Instance.Datas.ContainsKey(data.ID)) return true;

		Instance.DataList.Add(data);
		Instance.SaveAll();
		return false;
	}
	public static bool RemoveData(DataID id)
	{
		//지워졌다면 Save
		if (Instance.Datas.Remove(id))
		{
			Instance.SaveAll();
			return true;
		}
		return false;
	}
	#endregion
	
	#region
	public static InstanceData CreateByDataType(DataAsset createReqAsset, DataType type)
		=> CreateByDataType<InstanceData>(createReqAsset, type);
	public static T CreateByDataType<T>(DataAsset createReqAsset, DataType type)
		where T : InstanceData
	{
		return type switch
		{
			DataType.Lesson => new LessonInstanceData(createReqAsset, DataType.Lesson) as T,
			DataType.Activity => new ActivityInstanceData(createReqAsset, DataType.Activity) as T,
			DataType.Student => new StudentInstanceData(createReqAsset) as T,
			DataType.Professor => new ProfessorInstanceData(createReqAsset) as T,
			DataType.Worker => new WorkerInstanceData(createReqAsset) as T,
			DataType.Organization => new OrganizationInstanceData(createReqAsset, DataType.Organization) as T,
			DataType.School => new SchoolData(createReqAsset) as T,
			_ => null,
		};
	}
	#endregion

	#region Util
	public static DataID GetNewID(DataID assetID, DataType dataType)
	{
		for (int i = 1; i < DataIDStructure.indent_type; i++)
			if (!Instance.Datas.ContainsKey(new((int)assetID + (int)dataType + i)))
				return new((int)assetID + (int)dataType + i);
		return default;
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
			var json = line.Substring(split, line.Length - split);

			InstanceData newData = null;
			switch ((DataType)int.Parse(line.Substring(0, split)))
			{
				case DataType.Lesson:
					newData = JsonUtility.FromJson<LessonInstanceData>(json);
					break;
				case DataType.Activity:
					newData = JsonUtility.FromJson<ActivityInstanceData>(json);
					break;
				case DataType.Student:
					newData = JsonUtility.FromJson<StudentInstanceData>(json);
					break;
				case DataType.Professor:
					newData = JsonUtility.FromJson<ProfessorInstanceData>(json);
					break;
				case DataType.Worker:
					newData = JsonUtility.FromJson<WorkerInstanceData>(json);
					break;
				case DataType.Organization:
					newData = JsonUtility.FromJson<OrganizationInstanceData>(json);
					break;
				case DataType.School:
					newData = JsonUtility.FromJson<SchoolData>(json);
					break;
			}
			DataList.Add(newData);
		}
		//Debug.Log($"Load InstanceData Jsons");
	}
	public void SaveAll()
	{
		string jsons = string.Empty;
		foreach (var d in Instance.DataList)
		{
			// 한 줄에 (dataType + data(json) + \n)의 형태로 저장
			jsons += $"{(int)d.ID.GetDataType}{JsonUtility.ToJson(d)}\n";
		}
		File.WriteAllText(FilePath, jsons);

		Debug.Log($"Save InstanceData Jsons to {FilePath}");
	}
	#endregion


	#region Serialize
	public void OnBeforeSerialize()
	{
		Datas.Clear();
		
		for (int i=0; i<DataList.Count; i++)
			if(DataList[i] == null)
			{
				Debug.LogError("data is null. i'll remove it");
				DataList.RemoveAt(i);
				i--;
			}
			else if(Datas.ContainsKey(DataList[i].ID)) Debug.LogError($"duplicated Key {DataList[i].ID} - from {DataList[i].ID}");
			else Datas.Add(DataList[i].ID, DataList[i]);
	}
	public void OnAfterDeserialize() { }
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
	private void OnEnable() => LoadAll();
	private void OnDestroy()
	{
		if (Instance == this) _instance = null;
	}
	#endregion
}