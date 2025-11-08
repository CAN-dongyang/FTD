#if UNITY_EDITOR
/*
// DispositionMatrixEditor.cs (Assets/Editor/ 폴더 안에 저장)
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

// CustomEditor 속성은 어떤 클래스의 인스펙터를 커스텀할지 알려줌
[CustomEditor(typeof(DispositionMatrix))]
public class DispositionMatrixEditor : Editor
{
	// 인스펙터 창을 다시 그리는 메인 함수
	public override void OnInspectorGUI()
	{
		// 'target'은 현재 인스펙터에서 보고 있는 객체를 의미합니다.
		DispositionMatrix matrix = (DispositionMatrix)target;

		// 모든 Disposition enum 값들을 배열로 가져옵니다.
		var dispositions = Enum.GetValues(typeof(Disposition)).Cast<Disposition>().ToList();

		// 헤더 그리기 (교수 성향)
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("학생 \\ 교수", GUILayout.Width(80)); // 좌상단 빈칸
		foreach (var profDisposition in dispositions)
		{
			EditorGUILayout.LabelField(profDisposition.ToString(), GUILayout.Width(80));
		}
		EditorGUILayout.EndHorizontal();

		// 매트릭스 그리기
		foreach (var studentDisposition in dispositions)
		{
			EditorGUILayout.BeginHorizontal();
			// 학생 성향 이름 표시
			EditorGUILayout.LabelField(studentDisposition.ToString(), GUILayout.Width(80));

			// 해당 학생 성향에 대한 데이터 행(row)을 찾거나 새로 만듭니다.
			var row = matrix.matrix.FirstOrDefault(r => r.studentDisposition == studentDisposition);
			if (row == null)
			{
				row = new DispositionSynergyRow { studentDisposition = studentDisposition };
				matrix.matrix.Add(row);
			}

			// 각 교수 성향과의 시너지 값을 입력받는 필드를 그립니다.
			foreach (var profDisposition in dispositions)
			{
				var synergy = row.professorSynergies.FirstOrDefault(s => s.professorDisposition == profDisposition);
				if (synergy == null)
				{
					synergy = new SynergyValue { professorDisposition = profDisposition, multiplier = 1.0f };
					row.professorSynergies.Add(synergy);
				}

				// float 값을 입력받는 필드 생성
				synergy.multiplier = EditorGUILayout.FloatField(synergy.multiplier, GUILayout.Width(80));
			}
			EditorGUILayout.EndHorizontal();
		}

		// 변경 사항이 있으면 저장하라고 알려줍니다.
		if (GUI.changed)
		{
			EditorUtility.SetDirty(matrix);
		}
	}
}
*/
#endif