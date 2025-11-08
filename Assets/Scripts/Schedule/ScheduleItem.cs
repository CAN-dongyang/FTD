using Unity.Entities;
using Unity.Mathematics;

// C#의 DayOfWeek와 헷갈리지 않도록 게임 내 요일을 정의합니다.
public enum GameDayOfWeek
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

// 시간표의 한 줄에 해당하는 데이터입니다. (DOTS 버퍼용)
public struct ScheduleEntry : IBufferElementData
{
    public GameDayOfWeek Day;
    public float StartTime;
    public float3 TargetPosition;
}

// 현재 몇 번째 일정을 수행하고 있는지 기억하는 데이터입니다. (DOTS 컴포넌트)
public struct CurrentScheduleIndex : IComponentData
{
    public int Value;
}

// 인스펙터 창에서 시간과 위치를 한 세트로 보여주기 위한 도우미 클래스입니다. (MonoBehaviour용)
[System.Serializable]
public class ScheduleItem
{
    public float StartTime;
    public float3 TargetPosition;
}
