using Unity.Entities;
using UnityEngine;

public class OrganizationAuthoring : MonoBehaviour
{
	[SerializeField] private OrganizationAsset asset;
	[Tooltip("asset에 의해 초기화")]
	[SerializeField] private OrganizationInstanceData data;

	public InstanceDataID ID => data.ID;

	class OrganizationBaker : Baker<OrganizationAuthoring>
	{
		public override void Bake(OrganizationAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

			// Instance 데이터가 없다면 RuntimeDataManager에게 Data 생성 요청

			AddComponent(entity, new OrganizationComponentData()
			{
				InstanceID = authoring.data.ID
			});
		}
	}
}