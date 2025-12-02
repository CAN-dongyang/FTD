using System;
using UnityEngine;

/// <summary>
/// 가변 Instance 데이터
/// 
/// 생성 시 Manager에 자신을 등록한다
/// 파괴 시 Dispose()를 호출해야 하며 Manager에 등록한 id를 반납한다
/// </summary>
[Serializable]
public abstract class InstanceData : IDisposable
{
	[SerializeField] private DataID _id;
	public DataID ID => _id;
	public DataAsset Asset => InstanceDataManager.GetAsset(_id);

	private bool _disposed = false;

	/// <summary>
	/// 에셋을 사용하여 초기화. Override시 추가 초기화 가능.
	/// 
	/// Manager에게 새로운 ID를 요청하고 자신을 등록.
	/// </summary>
	public InstanceData(DataAsset asset, DataType type)
	{
		_id = InstanceDataManager.GetNewID(asset.ID, type);
		InstanceDataManager.AddData(this);
	}
	~InstanceData() { if(!_disposed) Dispose(); }

	/// <summary>
	/// 자신의 Instances ID를 Manager에게 반납한다
	/// </summary>
	public void Dispose()
	{
		if(!_disposed)
		{
			InstanceDataManager.RemoveData(ID);
			_disposed = true;
		}
	}
}