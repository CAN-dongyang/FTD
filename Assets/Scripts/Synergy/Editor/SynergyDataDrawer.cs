#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SynergyData))]
public class SynergyDataDrawer : PropertyDrawer
{
	private float _lineHeight = 24f;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return _lineHeight * 5f;
	}

	public override void OnGUI(Rect boundsRect, SerializedProperty property, GUIContent label)
	{
		var id = property.FindPropertyRelative("id");
		var idor = property.FindPropertyRelative("idOrStatType");
		var mul = property.FindPropertyRelative("multiplier");
		var err = property.FindPropertyRelative("errorRange");
		var add = property.FindPropertyRelative("adder");

		Rect pos = new Rect(boundsRect.xMin, boundsRect.yMin, boundsRect.width, _lineHeight);
		EditorGUI.BeginProperty(boundsRect, label, property);
		{
			// ID Field (수정 불가)
			EditorGUI.LabelField(pos, $"ID : {id.longValue}");
			pos.x = boundsRect.xMax - 84f; // x로 이동

			// idor 값에서 상태를 검사
			// stat인 경우 : 0 ~ StatType.End 값까지
			// id인 경우 : -1이거나 ID 값				-1은 null일 때 stat과 구분하기 위한 값
			bool isStatValue = !(idor.intValue < 0 || idor.intValue > (int)StatType.End);

			// Toggle Field
			isStatValue = EditorGUI.Toggle(pos, isStatValue);
			pos.x += 16f;
			EditorGUI.LabelField(pos, "Use Stat?");
			pos.y += _lineHeight; // y로 이동
			pos.height = EditorGUIUtility.singleLineHeight; // height 조정

			// 포지션 조정
			// x를 Label 크기만큼 띄우고 width 조정
			pos.x = boundsRect.xMin + EditorGUIUtility.labelWidth;
			pos.width = boundsRect.width - EditorGUIUtility.labelWidth;

			if (isStatValue)
			{
				// 값을 0 ~ StatType.End 사이로 고정시켜준다
				idor.intValue = Mathf.Clamp(idor.intValue, 0, (int)StatType.End);

				// Enum 클래스에서 StatType으로 변환 후 int로 변환
				idor.intValue = (int)(StatType)EditorGUI.EnumPopup(pos, (StatType)idor.intValue);
			}
			else
			{
				// 리소스를 다 뒤진다
				var synergy = Resources.LoadAll<SynergyProperty>("Entity/Synergy").FirstOrDefault(s => s.ID == idor.intValue);

#pragma warning disable CS0618 // 이 메서드 사용 안 되는 거 아는데 로그에 띄우지 마라고 표시
				synergy = EditorGUI.ObjectField(pos, synergy, typeof(SynergyProperty)) as SynergyProperty;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

				// -1은 null
				// enum StatType과 구분하기 위해 음수로 지정
				idor.intValue = (int)(synergy ? synergy.ID : -1);
			}
			// x 초기화. width는 label 크기만큼
			pos.x = boundsRect.xMin;
			pos.width = EditorGUIUtility.labelWidth;

			// 저장될 idor 값 출력 (직접 수정 불가)
			EditorGUI.LabelField(pos, $"ID : {idor.intValue}");

			pos.width = boundsRect.width; // width 초기화
			pos.y += _lineHeight; // y로 이동

			// 값 필드
			mul.floatValue = EditorGUI.FloatField(pos, "multiplier", mul.floatValue);
			pos.y += _lineHeight;

			err.floatValue = EditorGUI.FloatField(pos, "error Range", err.floatValue);
			pos.y += _lineHeight;

			add.floatValue = EditorGUI.FloatField(pos, "adder", add.floatValue);
			pos.y += _lineHeight;
		}
		EditorGUI.EndProperty();
	}
}
#endif