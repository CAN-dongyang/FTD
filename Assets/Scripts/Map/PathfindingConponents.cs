using Unity.Entities;
using Unity.Mathematics;

// 길찾기를 요청하는 일회성 컴포넌트입니다.
public struct PathRequest : IComponentData
{
    public float3 StartPosition;
    public float3 EndPosition;
}

// 계산된 경로의 각 경유지를 저장하는 버퍼입니다.
[InternalBufferCapacity(128)]
public struct PathWaypoint : IBufferElementData
{
    public float3 Position;
}

// 현재 경로의 몇 번째 경유지로 가고 있는지 기억하는 컴포-넌트입니다.
public struct CurrentPathIndex : IComponentData
{
    public int Value;
}

// 경로 계산은 끝났지만, 아직 출발 시간이 되지 않은 학생임을 나타냅니다.
public struct PendingMovement : IComponentData
{
    public float ScheduledStartTime; // 원래 도착해야 할 시간
}

