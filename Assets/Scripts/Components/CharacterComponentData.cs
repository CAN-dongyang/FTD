using Unity.Entities;

// 조직원 정보 (학생, 근로자 등)
public struct CharacterComponentData : IComponentData
{
	public InstanceDataID InstanceID;              // 자신의 RuntimeData ID
	public InstanceDataID OrganizationEntityID;   // 자신이 속한 조직 엔티티 Instance
}