using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement; // 씬 관리용
using UnityEditor.SceneManagement; // 씬 이동/저장용

public class PlayerSetup : EditorWindow
{
    static string prefabPath = "Assets/Resources/PlayerPrefab.prefab";

    [MenuItem("Tools/Setup Player (All Scenes)")]
    public static void SetupPlayer()
    {
        bool confirm = EditorUtility.DisplayDialog("플레이어 일괄 생성",
            "Build Profiles에 등록된 모든 씬에 'PlayerPrefab'을 생성합니다\n" + 
            "진행하겠습니까?", "확인", "취소");
        if (!confirm) return;

        // 1. PlayerData 에셋 생성 확인
        string path = "Assets/Resources/PlayerData.asset";
        PlayerData data = AssetDatabase.LoadAssetAtPath<PlayerData>(path);

        if (data == null)
        {
            data = ScriptableObject.CreateInstance<PlayerData>();
            // 기본값 설정
            data.money = 1000;
            
            // 폴더가 없으면 생성
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            Debug.Log("Created new PlayerData asset at " + path);
        }

        // 2. 플레이어 프리팹 준비
        GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        // 프리팹이 없으면 새로 만듭니다
        if (prefabAsset == null)
        {
            // 임시 게임오브젝트 생성
            GameObject playerObj = new GameObject("Player");
            playerObj.tag = "Player";
            
            // 컴포넌트 추가 
            Player player = playerObj.AddComponent<Player>();
            Rigidbody2D rb = playerObj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            playerObj.AddComponent<CircleCollider2D>();

            SpriteRenderer sr = playerObj.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 10;

            // 스크립트 변수 연결 (SerializedObject 사용)
            SerializedObject so = new SerializedObject(player);
            so.FindProperty("_data").objectReferenceValue = data;
            so.FindProperty("_rigidbody").objectReferenceValue = rb;
            so.ApplyModifiedProperties();

            //  만든 오브젝트를 프리팹 파일로 저장
            prefabAsset = PrefabUtility.SaveAsPrefabAsset(playerObj, prefabPath);
            
            // 임시로 만든 건 삭제 (프리팹만 있으면 됨)
            DestroyImmediate(playerObj);
            
            Debug.Log("Created Player Prefab at " + prefabPath);
        }

        // 3. 모든 씬을 돌면서 프리팹 배치
        string currentScenePath = SceneManager.GetActiveScene().path;
        EditorSceneManager.SaveOpenScenes(); // 현재 작업중인 씬 저장

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue; // 체크 해제된 씬은 패스

            // 씬 열기
            Scene openedScene = EditorSceneManager.OpenScene(scene.path);
            
            // 씬에 이미 Player가 있는지 확인
            Player existingPlayer = Object.FindFirstObjectByType<Player>();

            if (existingPlayer == null)
            {
                // 프리팹 생성 (InstantiatePrefab을 써야 연결이 유지됨)
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
                instance.transform.position = Vector3.zero; // 위치 초기화
                
                // 씬 저장 예약
                EditorSceneManager.MarkSceneDirty(openedScene);
                EditorSceneManager.SaveScene(openedScene);
                Debug.Log($"[{openedScene.name}] 플레이어 생성 완료.");
            }
        }

        // 원래 작업하던 씬으로 돌아오기
        if (!string.IsNullOrEmpty(currentScenePath))
        {
            EditorSceneManager.OpenScene(currentScenePath);
        }

        Debug.Log("모든 씬 작업 끝!");
    }
}

