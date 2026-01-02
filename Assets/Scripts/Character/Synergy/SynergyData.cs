using System;

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