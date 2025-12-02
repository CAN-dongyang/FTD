using System;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class SynergyProperty : DataAsset, ISynergy
{
	[SerializeField] private Sprite _icon;
	public Sprite Icon => _icon;
	[SerializeField] private List<SynergyData> _synergyDatas;
	public SynergyData GetSynergyData(ISynergy other) => GetSynergyData(other.ID);
	public SynergyData GetSynergyData(int otherID) => _synergyDatas.Find(data => data.idOrStatType == otherID);

#if UNITY_EDITOR
	private void OnValidate() // 스크립트가 로드되거나 값이 변경될 때 호출
	{
		// SynergyData의 id 필드를 무조건 내 자신의 ID로 채운다
		foreach (var d in _synergyDatas)
			if (d.id != ID) d.id = ID;
	}
#endif
}

[Serializable]
public class SynergyData
{
	public int id;
	public int idOrStatType;
	public float multiplier;
	public float errorRange; // 오차범위 +-
	public float adder;

	public SynergyData(int id, int idOrStatType, float multiplier, float errorRange, float adder)
	{
		this.id = id;
		this.idOrStatType = idOrStatType;
		this.multiplier = multiplier;
		this.errorRange = errorRange;
		this.adder = adder;
	}
}