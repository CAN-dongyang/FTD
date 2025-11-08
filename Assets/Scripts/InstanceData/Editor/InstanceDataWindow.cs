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

		window.target = new SerializedObject(InstanceDataManager.Instance);
		InstanceDataManager.Instance.LoadAll();
		window.Refresh();

		window.Show();
	}
	SerializedObject target = null;

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
	EntityAsset createReqAsset = null;
	InstanceDataType selectedDataType = InstanceDataType.None;

	[Obsolete] // Warning 표시하자 마라고. ObjectField 쓸 거라고
	void CreateField()
	{
		EditorGUILayout.BeginVertical();
		{
			createReqAsset = EditorGUILayout.ObjectField("Create New Data", createReqAsset, typeof(EntityAsset)) as EntityAsset;
			if (createReqAsset)
			{
				selectedDataType = (InstanceDataType)EditorGUILayout.EnumPopup(selectedDataType);

				if (selectedDataType == InstanceDataType.None) // + condition : 선택한 asset에서 선택한 data를 만들 수 있는가?
				{
				}
				else if (GUILayout.Button("Create"))
				{
					// is Valid?
					switch (selectedDataType)
					{
						case InstanceDataType.Lesson:
							{
								new LessonInstanceData(createReqAsset);
							}
							break;
						case InstanceDataType.Work:
							{
								new WorkInstanceData(createReqAsset);
							}
							break;
						case InstanceDataType.Student:
							{
								new StudentInstanceData(createReqAsset);
							}
							break;
						case InstanceDataType.Professor:
							{
								new ProfessorInstanceData(createReqAsset);
							}
							break;
						case InstanceDataType.Worker:
							{
								new WorkerInstanceData(createReqAsset);
							}
							break;
						case InstanceDataType.Organization:
							{
								new OrganizationInstanceData(createReqAsset);
							}
							break;
					}
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
		var list = target.FindProperty("DataList");

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
		target.Update();
		AssetDatabase.Refresh();
		Repaint();
	}
}
#endif