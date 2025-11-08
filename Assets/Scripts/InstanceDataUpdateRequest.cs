using Unity.Entities;

// 해당 RuntimeData를 업데이트 요청하는 Tag Component
public class UpdateDataRequest : IComponentData
{
	public int InstanceID;
}