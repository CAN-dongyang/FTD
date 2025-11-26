#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

// Inspector에서 ID를 수정 불가하지만 표시는 하기 위한 Editor
[CanEditMultipleObjects]
[CustomEditor(typeof(EntityAsset), editorForChildClasses: true)]
public class EntityAssetEditor : Editor
{
	private SerializedProperty _idProperty;

	// InspectorGUI가 생성될 때 한 번만 id를 가져옴
	public override VisualElement CreateInspectorGUI()
	{
		_idProperty = serializedObject.FindProperty("_id");
		return base.CreateInspectorGUI();
	}

	public override void OnInspectorGUI()
	{
		// 먼저 모든 내용을 그리고 (child 포함)
		base.OnInspectorGUI();
		
		EditorGUI.BeginDisabledGroup(true);
		{
			EditorGUILayout.Space(10);
			EditorGUILayout.IntField("Asset ID", _idProperty.intValue);
		}
		EditorGUI.EndDisabledGroup();
	}
}
#endif