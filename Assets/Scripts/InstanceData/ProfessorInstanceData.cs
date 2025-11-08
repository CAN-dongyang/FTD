using UnityEngine;

public class ProfessorInstanceData : CharacterInstanceData
{
	public override InstanceDataType DataType => InstanceDataType.Professor;
	public ProfessorInstanceData(EntityAsset asset) : base(asset) { }

	[Header("Professor")]
	public int scheduleDatas;
}