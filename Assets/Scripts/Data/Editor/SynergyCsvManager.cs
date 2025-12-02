#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// Load / Save Synergy Datas to CSV
public partial class SynergyCsvManager
{
	const string assetDirPath = "Entity/Synergy";
	const string saveFileName = "synergySaves.csv";
	const string dataFieldName = "_synergyDatas";

	//[MenuItem(itemName: "FTD/Save All Synergy to csv")]
	public static void SaveAllSynergy()
	{
		// 모든 Synergy를 List 타입으로 로드
		var list = Resources.LoadAll<SynergyProperty>(assetDirPath).ToList();
		if (list.Count == 0)
		{
			Debug.LogError($"'Resources/{assetDirPath}'에서 SynergyProperty들을 찾지 못했습니다.");
			return;
		}

		// 타입에서 필드들을 찾아 할당
		Type t = typeof(SynergyProperty);
		FieldInfo dataField = t.GetField(dataFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
		if (dataField == null)
		{
			Debug.LogError($"{t}타입에서 {dataFieldName} 필드를 찾을 수 없습니다.");
			return;
		}

		// 모든 시너지 값들을 중복되지 않는 선에서 모두 찾음
		List<SynergyData> saves = new();
		int notSaved = 0;

		foreach (var asset in list)
		{
			// foreach 내부에선 오른쪽 값을 저장하므로 실제로 GetValue() as ...는 한 번만 호출된다. 낭비가 아니다
			foreach (var data in dataField.GetValue(asset) as List<SynergyData>)
			{
				SynergyData duplicated;

				// 두 id 쌍의 값이 중복되는지 체크
				if (null != (duplicated = saves.Find(d =>
					(d.id == data.id && d.idOrStatType == data.idOrStatType) ||
					(d.id == data.idOrStatType && d.idOrStatType == data.id))))
				{
					// 알림!
					string warning = "중복된 Synergy 데이터가 있습니다.\n";
					warning += $"<color=green>{asset.DisplayName}</color>의 데이터 id 쌍 : ({data.id},{data.idOrStatType}) <color=orange>값 : {data.multiplier}</color>, ";
					warning += $"<color=green>{duplicated.id}</color>의 id 쌍 : ({duplicated.id},{duplicated.idOrStatType}) <color=orange>값 : {duplicated.multiplier}</color>";

					Debug.LogWarning(warning);
					notSaved++;
				}
				// 유효한 id가 아니거나 idOrStatType이 음수거나 mul 값이 0일 경우
				else if (data.idOrStatType < 0 || data.multiplier.Equals(0f))
				{
					string warning = "유효하지 않은 ID 또는 값이 있습니다.\n";
					warning += $"ID 쌍 : <color=yellow>({data.id},{data.idOrStatType})</color>, ";
					warning += $"값 : <color =orange>{data.multiplier}</color>";

					Debug.LogWarning(warning);
					notSaved++;
				}
				else saves.Add(data);
			}
		}

		if (saves.Count == 0)
		{
			Debug.Log("Synergy Data가 포함된 Asset이 없습니다. 저장을 종료합니다.");
			return;
		}

		string csv = "id,idOrStatType,multiplier,errorRange,adder"; // 필드명

		foreach (var data in saves)
			csv += $"\n{data.id},{data.idOrStatType},{data.multiplier},{data.errorRange},{data.adder}"; // write a record
		// 막 줄에 빈 행이 남지 않도록 데이터가 있을 때에만 개행(\n)

		string filePath = $"Assets/Resources/{assetDirPath}/{saveFileName}";
		File.WriteAllText(filePath, csv);

		AssetDatabase.Refresh(); // 새로고침

		string log = $"<color=green>{saves.Count}개</color>의 SynergyData가 저장되었습니다.\n";
		log += $"저장되지 못한 레코드 개수 : <color=yellow>{notSaved}</color>";

		Debug.Log(log);
		Debug.Log($"저장을 완료했습니다. {filePath}\n이 데이터대로 진행하기를 원하신다면 FTD/LoadAllSynergy를 호출해주세요.");
	}

	//[MenuItem(itemName: "FTD/Load All Synergy from csv")]
	public static void LoadAllSynergy()
	{
		// save csv 파일 로드
		string filePath = $"Assets/Resources/{assetDirPath}/{saveFileName}";
		if (!File.Exists(filePath))
		{
			Debug.LogError($"{filePath}를 찾지 못했습니다. 세이브 후 진행해주세요.");
			return;
		}

		// 모든 Synergy를 List 타입으로 로드
		var list = Resources.LoadAll<SynergyProperty>(assetDirPath).ToList();
		if (list.Count == 0)
		{
			Debug.LogError($"'Resources/{assetDirPath}'에서 SynergyProperty들을 찾지 못했습니다.");
			return;
		}

		// 타입에서 필드들을 찾아 할당
		Type t = typeof(SynergyProperty);
		FieldInfo dataField = t.GetField(dataFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
		if (dataField == null)
		{
			Debug.LogError($"{t}타입에서 {dataFieldName} 필드를 찾을 수 없습니다.");
			return;
		}

		// 모든 Synergy의 Data를 초기화
		list.ForEach(asset =>
		{
			dataField.SetValue(asset, new List<SynergyData>());

			EditorUtility.SetDirty(asset); // 저장이 필요함을 알림
		});

		string[] datas = File.ReadAllText(filePath).Split('\n');
		string[] records;
		int notLoaded = 0; // load error count
		int emptyLines = 1; // 필드명 또한 emptyLine

		// i=0 == csv의 필드명
		for (int i = 1; i < datas.Length; i++)
		{
			if (datas[i] == string.Empty)
			{
				emptyLines++;
				continue;
			}

			records = datas[i].Split(',');
			if (records.Length < 5)
			{
				Debug.LogWarning("csv에 비정상적인 길이의 record가 있습니다. 이 record는 Load하지 않습니다.");
				notLoaded++;
				continue;
			}

			// records에서 멤버들을 Parsing
			if (int.TryParse(records[0], out int id) &&
				int.TryParse(records[1], out int idOrStatType) &&
				float.TryParse(records[2], out float multiplier) &&
				float.TryParse(records[3], out float errorRange) &&
				float.TryParse(records[4], out float adder))
			{
				var find = list.Find(l => l.ID == id);

				// data list를 find에서 가져옴
				var dataList = dataField.GetValue(find) as List<SynergyData>;

				// list에 데이터 추가
				dataList.Add(new SynergyData(id, idOrStatType, multiplier, errorRange, adder));

				// dataList를 다시 find에 저장
				dataField.SetValue(find, dataList);
			}
			else // Parsing Error
			{
				notLoaded++;
			}
		}

		string log = $"<color=green>{datas.Length - emptyLines - notLoaded}개</color>의 SynergyData가 로드되었습니다.\n";
		log += $"로드되지 못한 레코드 개수 : <color=yellow>{notLoaded}</color>";

		Debug.Log(log);

		AssetDatabase.SaveAssets(); // 모든 dirty 에셋 저장
		AssetDatabase.Refresh(); // 새로고침
	}
}
#endif