#if UNITY_EDITOR
//	ID Structure는 EntityID.cs에 의존
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EntityAssetIDGenerator
{
	/// <summary>
	/// Entity Asset의 ID가 초기화되지 않았을 때 부여하기 위한 매핑 값
	/// fileName, displayName, type 중에서 가장 확실한 type을 이용했다
	/// 
	/// Editor 상태일 때만 매핑 값이 존재 (#if UNITY_EDITOR)
	/// </summary>
	private static readonly Dictionary<Type, EntityAssetType> _typeToEnumMap = new()
	{
		{ typeof(ActivityAsset), EntityAssetType.Activity },
		{ typeof(CharacterAsset), EntityAssetType.Character },
		{ typeof(OrganizationAsset), EntityAssetType.Organization },
		{ typeof(SynergyProperty), EntityAssetType.Synergy },
		{ typeof(Ability), EntityAssetType.Ability },
		{ typeof(Disposition), EntityAssetType.Disposition },
		{ typeof(JobProperty), EntityAssetType.Job },
		{ typeof(SkillProperty), EntityAssetType.Skill },
		{ typeof(Stat), EntityAssetType.Stat }, // 이 클래스는 ScriptableObject가 아니지만 ID 사용이 필요함
	};

	[MenuItem("FTD/Generate Entity Asset's ID")]
	public static void GenerateEntityIDs()
	{
		// Resources/Entity/ 하위의 모든 EntityAsset 로드
		var entities = Resources.LoadAll<EntityAsset>("Entity/");
		if (entities.Length == 0)
		{
			Debug.LogWarning("Assets/Resources/Entity/ 하위의 EntityAsset이 존재하지 않습니다. ID 생성을 종료합니다.");
			return;
		}

		// EntityAsset의 Private 필드인 _id를 불러올 방법. reflection
		FieldInfo idField = typeof(EntityAsset).GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance);
		if (idField == null)
		{
			Debug.LogWarning("EntityAsset 타입에 _id 필드가 존재하지 않거나 찾을 수 없습니다. ID 생성을 종료합니다.");
			return;
		}

		// Asset 타입 별 id index	(id 마지막의 iiii)
		Dictionary<EntityAssetType, int> indexDic = new();

		// log용 변수
		string dirtyLog = string.Empty;
		int dirtyCount = 0;

		foreach (var entity in entities)
		{
			var assetType = entity.GetType();
			var assetTypeEnum = _typeToEnumMap
				.FirstOrDefault(pair => pair.Key == assetType)
				.Value;

			if (assetTypeEnum == EntityAssetType.None)
				throw new KeyNotFoundException($"{assetType}에 매핑된 Enum Value를 찾을 수 없습니다.");

			if (!indexDic.ContainsKey(assetTypeEnum))
				indexDic.Add(assetTypeEnum, 1); // index 값은 1부터 시작

			int newID
				= (int)assetTypeEnum // Enum의 값은 assetType의 기본 값
				+ indexDic[assetTypeEnum]++; // index 값을 더해준 후 증가시킨다

			if (entity.ID != newID) // newID가 원래 ID와 다를 때
			{
				// entity의 _id 필드에 값 할당
				idField.SetValue(entity, newID);
				EditorUtility.SetDirty(entity); // 저장 대상임을 알림

				dirtyLog += entity.DisplayName + ";";
				dirtyCount++;
			}
		}

		string log = $"<color=green>{dirtyCount}개</color>의 Entity의 ID가 생성 또는 수정되었습니다.\n";
		log += $"수정이 필요하지 않은 Entity 개수 : <color=yellow>{entities.Length - dirtyCount}</color>";
		Debug.Log(log);

		if (dirtyCount > 0) Debug.Log("수정 목록 : " + dirtyLog);

		AssetDatabase.SaveAssets(); // 저장 호출
		AssetDatabase.Refresh(); // 새로고침
	}
}
#endif