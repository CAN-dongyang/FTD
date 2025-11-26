using System.Collections.Generic;
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
	public string FilePath => $"{Application.dataPath}/Resources/Entity/{fileName}";
#else
	public string FilePath => Application.persistentDataPath + fileName;
#endif

	[SerializeReference] // base class가 아닌 inherit type class로 저장됨
	[SerializeField]
	private List<InstanceData> DataList = new(); // 저장과 수정
	private Dictionary<DataID, InstanceData> Datas = new(); // 검색 ( O(1) )

	public static bool Contains(DataID id) => Instance.Datas.ContainsKey(id);
	#region Gets
	public static List<InstanceData> GetAllData() => Instance.DataList;
	public static InstanceData Get(DataID id)
	{
		return Instance.Datas.TryGetValue(id, out InstanceData value) ? value : null;
	}
	public static T Get<T>(DataID id) where T : InstanceData
	{
		return Instance.Datas.TryGetValue(id, out InstanceData value) ? value as T : null;
	}

	public static DataID GetNewID(int assetID, DataType dataType)
	{
		// sub type 값을 침범하기 전까지
		for (int i = 1; i < IDStructure.indent_s; i++)
			if (!Instance.Datas.ContainsKey(new(assetID + (int)dataType + i)))
				return new(assetID + (int)dataType + i);
		return default;
	}	
	#endregion

	#region DDL
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
	public static InstanceData CreateByDataType(EntityAsset createReqAsset, DataType type)
	{
		return type switch
		{
			DataType.Lesson => new LessonInstanceData(createReqAsset),
			DataType.Activity => new ActivityInstanceData(createReqAsset),
			DataType.Student => new StudentInstanceData(createReqAsset),
			DataType.Professor => new ProfessorInstanceData(createReqAsset),
			DataType.Worker => new WorkerInstanceData(createReqAsset),
			DataType.Organization => new OrganizationInstanceData(createReqAsset),
			DataType.School => new SchoolData(createReqAsset),
			_ => null,
		};
	}
	public static InstanceData ParseByDataType(InstanceData data, DataType type)
	{
		return type switch
		{
			DataType.Lesson => data as LessonInstanceData,
			DataType.Activity => data as ActivityInstanceData,
			DataType.Student => data as StudentInstanceData,
			DataType.Professor => data as ProfessorInstanceData,
			DataType.Worker => data as WorkerInstanceData,
			DataType.Organization => data as OrganizationInstanceData,
			DataType.School => data as SchoolData,
			_ => null,
		};
	}
	#endregion

	#region Save Load
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
		
		foreach (var data in DataList)
			if(data == null) Debug.LogError("data is null");
			else if(Datas.ContainsKey(data.ID)) Debug.LogError($"duplicated Key {data.ID}");
			else Datas.Add(data.ID, data);
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