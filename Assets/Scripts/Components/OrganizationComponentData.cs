using Unity.Entities;
using UnityEngine;

// 조직 정보 (학교, 회사 등)
public struct OrganizationComponentData : IComponentData
{
	public int InstanceID;			// 자신의 ID
}

public struct RoomComponentData : IBufferElementData
{
	public int parentID; // parent organization
	public BoundsInt bounds;
}