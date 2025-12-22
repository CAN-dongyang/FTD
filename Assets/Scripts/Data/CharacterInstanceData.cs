using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CharacterInstanceData : InstanceData
{
	// Stat의 배열 구조를 상수화. 생성자에서 할당하며 내부의 값은 조작 가능.
	public readonly Stat[] stats;
	// StatType 값과 배열 index는 무조건 동일하다
	public Stat GetStat(StatType type) => stats[(int)type];

	// 시너지 프로퍼티들
	[Header("Character")]
	public Ability[] aptitudes;
	public Ability[] interests;
	public Disposition[] dispositions;
	public List<SkillProperty> skills;

	// [family]의 [gen] 대손에 해당
	public int familyGeneration = 1;

	public DataID organization;

	public CharacterInstanceData(DataAsset asset, DataType type) : base(asset, type)
	{
		int statCount = (int)StatType.End;

		stats = new Stat[statCount];
		for (int i = 0; i < statCount; i++)
			stats[i] = new Stat((StatType)i);
	}
}

[Serializable]
public class StudentInstanceData : CharacterInstanceData
{
	[Header("Student")]
	public int scheduleDatas;

	public StudentInstanceData(DataAsset asset) : base(asset, DataType.Student) {}
}

[Serializable]
public class WorkerInstanceData : CharacterInstanceData
{
	[Header("Worker")]
	public JobProperty Job;
	public int payCost;

	public WorkerInstanceData(DataAsset asset) : base(asset, DataType.Worker) {}
}

[Serializable]
public class ProfessorInstanceData : CharacterInstanceData
{
	[Header("Professor")]
	public int scheduleDatas;

	public ProfessorInstanceData(DataAsset asset) : base(asset, DataType.Professor)
	{
	}
}