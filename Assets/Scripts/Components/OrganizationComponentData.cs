using Unity.Entities;

// 조직 정보 (학교, 회사 등)
public struct OrganizationComponentData : IComponentData
{
	public InstanceDataID InstanceID;			// 자신의 ID
}