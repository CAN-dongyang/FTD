using Unity.Entities;

// 현재 캐릭터가 수행중인 액션
public struct CurrentAction : IComponentData
{
    public int ActionID;
	public float StartTime;
	public float EndTime;
	public Entity TargetPlace;	// 액션 대상 (교실 등)
}

// 길찾기 요청 Tag Component
public struct PathfindingRequest : IComponentData
{
	public Entity TargetPlace; // 목적지 (교실 등)
}