using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CharacterAuthoring : MonoBehaviour
{
	[SerializeField] private CharacterAsset asset;
	[Tooltip("asset에 의해 초기화")]
	[SerializeField] private CharacterInstanceData data;
	[SerializeField] private InstanceDataType dataType;
	[SerializeField] private OrganizationAuthoring organization;

	public InstanceDataID ID => data.ID;

	class CharacterBaker : Baker<CharacterAuthoring>
	{
		public override void Bake(CharacterAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

			// Instance 데이터가 없다면 RuntimeDataManager에게 Data 생성 요청
			switch (authoring.dataType)
			{
				case InstanceDataType.Student:
					
				break;
				case InstanceDataType.Professor:
				break;
				case InstanceDataType.Worker:
				break;
			}
			
			AddComponent(entity, new CharacterComponentData()
			{
				InstanceID = authoring.data.ID,
				OrganizationEntityID = authoring.organization ? authoring.organization.ID : new(-1,-1)
			});
		}
	}
}