#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

public class InstanceDataWindow : EditorWindow
{
	[MenuItem("FTD/Instance Data Window")]
	static void Init()
	{
		var window = GetWindow<InstanceDataWindow>();
		window.minSize = new(800, 600);

		InstanceDataManager.Instance.LoadAll();
		window.Refresh();

		window.Show();
	}
	SerializedObject target = null;
	SerializedObject Target
	{
		get => target ??= new SerializedObject(InstanceDataManager.Instance);
	}

	[Obsolete] // Warning 표시하자 마라고. ObjectField 쓸 거라고
	public void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		{
			CreateField();
			EditField();
			ListField();
		}
		EditorGUILayout.EndVertical();

		target.ApplyModifiedProperties();
	}

	#region Sub Fields
	DataAsset createReqAsset = null;
	DataType selectedDataType = DataType.None;

	[Obsolete] // Warning 표시하자 마라고. ObjectField 쓸 거라고
	void CreateField()
	{
		EditorGUILayout.BeginVertical();
		{
			createReqAsset = EditorGUILayout.ObjectField("Create New Data", createReqAsset, typeof(DataAsset)) as DataAsset;
			if (createReqAsset)
			{
				selectedDataType = (DataType)EditorGUILayout.EnumPopup(selectedDataType);
				if(selectedDataType != DataType.None && !new DataID(selectedDataType).IsInstance)
				{
					Debug.LogWarning("선택한 데이터 타입이 Instance 타입이 아닙니다.");
				}

				// + condition : 선택한 asset에서 선택한 data를 만들 수 있는가?
				if (GUILayout.Button("Create"))
				{
					InstanceDataManager.CreateByDataType(createReqAsset, selectedDataType);
					Refresh();
				}
			}
		}
		EditorGUILayout.EndVertical();
	}

	bool editmode = false;
	void EditField()
	{
		EditorGUILayout.Space(10);
		{
			editmode = EditorGUILayout.Toggle("Edit Mode (Danger)", editmode);
			if (GUILayout.Button("Load Button"))
			{
				InstanceDataManager.Instance.LoadAll();
				Refresh();
			}
			if (GUILayout.Button("Save Button"))
			{
				InstanceDataManager.Instance.SaveAll();
				Refresh();
			}
		}
		EditorGUILayout.Space(10);
	}

	Vector2 scrollPos;
	void ListField()
	{
		var list = Target.FindProperty("DataList");

		EditorGUILayout.LabelField($"Instance Datas ({list.arraySize})");
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		{
			EditorGUI.BeginDisabledGroup(!editmode);
			{
				EditorGUILayout.PropertyField(list);
			}
			EditorGUI.EndDisabledGroup();
		}
		EditorGUILayout.EndScrollView();
	}
	#endregion

	void Refresh()
	{
		Target.Update();
		AssetDatabase.Refresh();
		Repaint();
	}
}
#endif