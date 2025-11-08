using UnityEngine;

public class StudentInstanceData : CharacterInstanceData
{
	public override InstanceDataType DataType => InstanceDataType.Student;
	public StudentInstanceData(EntityAsset asset) : base(asset) { }

	[Header("Student")]
	public int scheduleDatas;
}