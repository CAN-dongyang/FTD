using System;
using UnityEngine;

[Serializable]
public class WorkerInstanceData : CharacterInstanceData
{
	public override InstanceDataType DataType => InstanceDataType.Worker;
	public WorkerInstanceData(EntityAsset asset) : base(asset) { }

	[Header("Worker")]
	public JobProperty Job;
	public string organization;
	public int payCost;
}