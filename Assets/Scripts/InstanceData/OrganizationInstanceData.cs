public class OrganizationInstanceData : InstanceData
{
	public override InstanceDataType DataType => InstanceDataType.Organization;
	public OrganizationInstanceData(EntityAsset asset) : base(asset) { }
}