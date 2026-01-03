#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DataIDAttribute))]
public class DataIDDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		DataIDAttribute attr = (DataIDAttribute)attribute;
		SerializedProperty valueProp = property.FindPropertyRelative("Value");
		
		DataID id = new DataID((uint)valueProp.longValue);

		// 16진수 접두어와 함께 레이블 표시
		label.text = $"[{id}] {label.text}";
		
		// 인스펙터에서는 읽기 전용으로 표시 (자동 할당되니까!)
		GUI.enabled = false; 
		EditorGUI.PropertyField(position, valueProp, label);
		GUI.enabled = true;
	}
}
#endif