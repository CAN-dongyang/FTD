using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;

public class CharacterAuthoring : MonoBehaviour
{
	[SerializeField] private DataID findID;
	[SerializeField] private CharacterAsset asset;
	[SerializeField] private DataType dataType;

	[Tooltip("asset, dataType에 의해 초기화하거나 data에 입력된 ID로 찾음")]
	// base class가 아닌 inherit type class로 저장됨
	[SerializeReference]
	public InstanceData data;
	public OrganizationAuthoring organization;

	class CharacterBaker : Baker<CharacterAuthoring>
	{
		public override void Bake(CharacterAuthoring authoring)
		{
			if(authoring.dataType == DataType.None) return;
			
			if(authoring.findID.IsValid && InstanceDataManager.Contains(authoring.findID))
			{
				authoring.data = InstanceDataManager.GetData(authoring.findID);
			}
			else if(authoring.data != null && authoring.data.ID.IsValid && InstanceDataManager.Contains(authoring.data.ID))
			{
				authoring.findID = authoring.data.ID;
				authoring.data = InstanceDataManager.GetData(authoring.findID);
			}
			else if(authoring.asset != null && authoring.dataType != DataType.None)
			{
				authoring.data = InstanceDataManager.CreateByDataType(authoring.asset, authoring.dataType);

				Debug.Log($"Baker Create InstanceData to '{authoring.dataType}' in Asset '{authoring.asset.DisplayName}'");
			}
			else return; // quit

			var entity = GetEntity(TransformUsageFlags.Dynamic);

			// AddComponent(entity, new MoveSpeedComponent { Value = authoring.Speed });
			// AddComponent(entity, new CurrentScheduleIndex { Value = 0 });
			// 길찾기를 위해 CurrentPathIndex 컴포넌트를 추가합니다.
			// AddComponent(entity, new CurrentPathIndex { Value = -1 }); // -1은 경로가 없음을 의미
			AddComponent(entity, new CharacterComponentData()
			{
				InstanceID = authoring.data.ID,
				OrganizationEntityID = 0
			});
			AddComponent<IsAwakeTag>(entity);
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

