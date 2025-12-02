using Unity.Entities;
using UnityEngine;

public class InstanceAuthoring : MonoBehaviour
{
	[SerializeField] private DataID findID;
	[SerializeField] private DataAsset asset;
	[SerializeField] private DataType dataType;

	[Tooltip("asset, dataType에 의해 초기화하거나 data에 입력된 ID로 찾음")]
	// base class가 아닌 inherit type class로 저장됨
	[SerializeReference] public InstanceData data;

	class InstanceBaker : Baker<InstanceAuthoring>
	{
		public override void Bake(InstanceAuthoring authoring)
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
			AddComponent(entity, new InstanceComponentData()
			{
				InstanceID = authoring.data.ID
			});
		}
	}
}