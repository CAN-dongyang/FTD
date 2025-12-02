using Unity.Entities;

// 스케줄의 각 항목
public struct Schedule : IBufferElementData
{
    public float StartTime;     // 시작 시간 (GameTime.TimeOfDay)
    public float EndTime;
    public Entity AssignedRoom; // 할당된 공간 엔티티
}

// ----- 빠른 쿼리와 안전 수정을 위해 스케줄 버퍼를 아래처럼 분리 -----

// 현재 분기 스케줄 버퍼 (Organization 엔티티에 존재)
public struct CurrentQuaterSchedule : IBufferElementData
{
    public Schedule Value;
}

// 다음 분기 스케줄 버퍼 (Organization 엔티티에 존재하며 수정 가능)
public struct NextQuaterSchedule : IBufferElementData
{
    public Schedule Value;
}