using System;
using UnityEngine;

[Serializable]
public struct InstanceDataID : IEquatable<InstanceDataID>
{
	public int entityID, instanceID;

	public InstanceDataID(int entityID, int instanceID)
	{
		this.entityID = entityID;
		this.instanceID = instanceID;
	}
	public bool Equals(InstanceDataID other)
	{
		return entityID == other.entityID && instanceID == other.instanceID;
	}
}

/// <summary>
/// 가변 Runtime 데이터
/// 
/// 생성 시 Manager에 자신을 등록한다
/// 파괴 시 Dispose()를 호출해야 하며 Manager에 등록한 id를 반납한다
/// </summary>
[Serializable]
public abstract class InstanceData : IDisposable
{
	[SerializeField] private InstanceDataID _id;
	public InstanceDataID ID => _id;
	/// <summary>
	/// 상속 시 Data Type을 반드시 명시해야 한다
	/// </summary>
	public abstract InstanceDataType DataType { get; }

	private EntityAsset _asset; // non-Serialized
	public EntityAsset Asset => _asset;

	/// <summary>
	/// 에셋을 사용하여 초기화. Override시 추가 초기화 가능.
	/// 
	/// Manager에게 새로운 ID를 요청하고 자신을 등록.
	/// </summary>
	public InstanceData(EntityAsset asset)
	{
		_id = InstanceDataManager.GetNewInstanceID(asset.ID, DataType);
		_asset = asset;
		InstanceDataManager.AddData(this);
	}

	/// <summary>
	/// 자신의 Instances ID를 Manager에게 반납한다
	/// </summary>
	public void Dispose()
	{
		InstanceDataManager.RemoveDataID(ID);
	}
}