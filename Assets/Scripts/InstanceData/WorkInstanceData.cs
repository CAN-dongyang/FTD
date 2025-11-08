public class WorkInstanceData : InstanceData
{
	public override InstanceDataType DataType => InstanceDataType.Work;
	public WorkInstanceData(EntityAsset asset) : base(asset) { }
}