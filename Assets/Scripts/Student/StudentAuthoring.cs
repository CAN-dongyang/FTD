using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class StudentAuthoring : MonoBehaviour
{
    public float Speed;

    [Header("Weekly Schedule (Mon-Fri)")]
    public List<ScheduleItem> MondaySchedule = new List<ScheduleItem>();
    public List<ScheduleItem> TuesdaySchedule = new List<ScheduleItem>();
    public List<ScheduleItem> WednesdaySchedule = new List<ScheduleItem>();
    public List<ScheduleItem> ThursdaySchedule = new List<ScheduleItem>();
    public List<ScheduleItem> FridaySchedule = new List<ScheduleItem>();

    class StudentBaker : Baker<StudentAuthoring>
    {
        public override void Bake(StudentAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new MoveSpeedComponent { Value = authoring.Speed });
            AddComponent(entity, new CurrentScheduleIndex { Value = 0 });
            // 길찾기를 위해 CurrentPathIndex 컴포넌트를 추가합니다.
            AddComponent(entity, new CurrentPathIndex { Value = -1 }); // -1은 경로가 없음을 의미
            AddComponent<IsAwakeTag>(entity);

            var scheduleBuffer = AddBuffer<ScheduleEntry>(entity);

            AddScheduleEntries(scheduleBuffer, authoring.MondaySchedule, GameDayOfWeek.Monday);
            AddScheduleEntries(scheduleBuffer, authoring.TuesdaySchedule, GameDayOfWeek.Tuesday);
            AddScheduleEntries(scheduleBuffer, authoring.WednesdaySchedule, GameDayOfWeek.Wednesday);
            AddScheduleEntries(scheduleBuffer, authoring.ThursdaySchedule, GameDayOfWeek.Thursday);
            AddScheduleEntries(scheduleBuffer, authoring.FridaySchedule, GameDayOfWeek.Friday);
        }

        private void AddScheduleEntries(DynamicBuffer<ScheduleEntry> buffer, List<ScheduleItem> schedule, GameDayOfWeek day)
        {
            foreach (var item in schedule)
            {
                buffer.Add(new ScheduleEntry { Day = day, StartTime = item.StartTime, TargetPosition = item.TargetPosition });
            }
        }
    }
}

