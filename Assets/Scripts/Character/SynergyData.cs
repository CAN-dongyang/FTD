using System;
using UnityEngine;

[Serializable]
public class SynergyData
{
	public ScriptableObject target;

	[Range(0.5f, 1.5f)]
	public float multiplier;
	[Range(0f, 5f), Tooltip("오차범위 +-")]
	public float errorRange;
	[Range(-5f, 5f)] public float adder;

	public SynergyData(float multiplier, float errorRange, float adder)
	{
		this.multiplier = multiplier;
		this.errorRange = errorRange;
		this.adder = adder;
	}
}