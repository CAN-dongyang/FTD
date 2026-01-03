#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

public class DataIDDashboard : EditorWindow
{
    private AssetType selectedType = AssetType.Character;
    private BaseAssetSO selectedAsset;
    private Vector2 scrollPos1, scrollPos2;

    [MenuItem("Window/DataID Dashboard")]
    public static void ShowWindow() => GetWindow<DataIDDashboard>("DataID Dashboard");

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        // --- 1. 좌측: AssetType 선택 ---
        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(150));
        GUILayout.Label("Asset Types", EditorStyles.boldLabel);
        foreach (AssetType t in Enum.GetValues(typeof(AssetType)))
        {
            if (GUILayout.Toggle(selectedType == t, t.ToString(), "Button"))
            {
                selectedType = t;
            }
        }
        EditorGUILayout.EndVertical();

        // --- 2. 중앙: 해당 타입의 에셋 리스트 (Registry에서 가져옴) ---
        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(250));
        GUILayout.Label($"{selectedType} Assets", EditorStyles.boldLabel);
        
        // 실제 프로젝트에서는 AssetDatabase로 Registry를 찾아와야 함
        var registry = FindRegistry(selectedType); 
        if (registry != null)
        {
            scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1);
            for (int i = 0; i < registry.assets.Count; i++)
            {
                var asset = registry.assets[i];
                string label = $"[AI:{i:D2}] {(asset != null ? asset.name : "Empty")}";
                if (GUILayout.Toggle(selectedAsset == asset, label))
                {
                    selectedAsset = asset;
                }
            }
            EditorGUILayout.EndScrollView();
            
            if (GUILayout.Button("Auto-Assign AI (Registry)")) { registry.SyncIndices(); }
        }
        EditorGUILayout.EndVertical();

        // --- 3. 우측: 에셋 내부 데이터 상세 정보 ---
        EditorGUILayout.BeginVertical(GUI.skin.box);
        if (selectedAsset != null)
        {
            GUILayout.Label($"Data in: {selectedAsset.name}", EditorStyles.boldLabel);
            GUILayout.Space(10);

            scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);
            DrawDataList(selectedAsset);
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Fix Persistent IDs (Save Support)")) 
            { 
                // 여기서 데이터 내부의 persistentDI를 할당하는 로직 실행
                //selectedAsset.FixDataIndices(); 
            }
        }
        else
        {
            GUILayout.Label("Select an asset to see data");
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawDataList(BaseAssetSO asset)
    {
        // 리플렉션이나 인터페이스를 통해 에셋 내부의 데이터를 가져와서 그림
        // 예시: 0x AA II TT DI 형태의 결과값을 레이블로 출력
        // EditorGUILayout.LabelField($"Full ID: 0x{id.Value:X8} | Name: {data.name}");
    }

    private AssetRegistry FindRegistry(AssetType type) 
    {
        // AssetDatabase.FindAssets를 사용하여 실제 Registry 파일을 찾는 로직 필요
		var assets = AssetDatabase.FindAssets("t:AssetRegistry")
			.Where(hex => GUID.TryParse(hex, out GUID result))
			.Select(hex =>
			{
				GUID.TryParse(hex, out GUID result);
				return AssetDatabase.LoadAssetByGUID<AssetRegistry>(result);
			});
		foreach(var a in assets)
		{
			if(a.registryType == type) return a;
		}
        return null;
    }
}
#endif