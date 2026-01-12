using UnityEngine;
using UnityEditor;

public class PlayerSetup : EditorWindow
{
    [MenuItem("Tools/Setup Player")]
    public static void SetupPlayer()
    {
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

        // 2. 씬에 플레이어 오브젝트 생성 확인
        Player player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            GameObject playerObj = new GameObject("Player");
            playerObj.tag = "Player"; // 태그 설정 중요
            
            // 컴포넌트 추가
            player = playerObj.AddComponent<Player>();
            Rigidbody2D rb = playerObj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; // 2D 탑다운 게임 가정
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            playerObj.AddComponent<CircleCollider2D>();

            // SpriteRenderer 추가 및 레이어 순서 설정
            SpriteRenderer sr = playerObj.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 10; // 맵(0)보다 높은 숫자로 설정하여 위로 올림
            Debug.Log("Added SpriteRenderer with Sorting Order 10.");

            // 스크립트 내부 필드(private)는 Reflection이나 SerializedObject로 설정
            SerializedObject so = new SerializedObject(player);
            so.FindProperty("_data").objectReferenceValue = data;
            so.FindProperty("_rigidbody").objectReferenceValue = rb;
            so.ApplyModifiedProperties();

            Selection.activeGameObject = playerObj;
            Debug.Log("Player object created and configured in the scene.");
        }
        else
        {
            Debug.Log("Player already exists in the scene.");
        }
    }
}
