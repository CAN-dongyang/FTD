// Stat.cs
using System;
using UnityEngine;

public enum StatType
{
	Strength,   // 근력
	Agility,    // 민첩
	Technique,  // 기술
	Intelligence, // 지력
	Mentality,  // 정신력
	Creativity,  // 창의력
	Charm, // 매력
	Dignity, // 기품
	Morality, // 도덕성
	End // 배열 카운트
}

[Serializable]
public struct Stat : ISynergy
{
	// ID 호환성을 위해 Type의 값을 ID로 한다
	public readonly DataID ID => new((int)Type);

	[SerializeField] private StatType _type;
	public readonly StatType Type => _type;

	// 실제 소수점 계산을 위한 변수
	[SerializeField] private float _value;
	// 외부에서 값을 읽을때 사용하는 프로퍼티
	public float Value
	{
		get => _value;
		set => _value = value;
	}

	// int 형으로 명시적(explicit) 형변환 가능. (float)s
	// int로 형변환 시에는 내림을 적용
	public static explicit operator int(Stat s) => Mathf.FloorToInt(s.Value);

	// 스텟이 생성될 때 초기값을 설정해야 함
	public Stat(StatType type, int startingValue = 0)
	{
		_type = type;
		_value = startingValue;
	}
}