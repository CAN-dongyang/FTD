using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data의 ID와 Resource를 연결하는 클래스.
/// 
/// <c>DataID</c> 등에서 내부적으로 사용.
/// </summary>
public class DataManager : MonoBehaviour
{
	public static DataManager Instance { get; private set; }
	private void Awake() => Instance = this;

	private Dictionary<uint, ScriptableObject> _assets = new();
	private Dictionary<uint, object> _datas = new();

	// --- Register ---
	public void RegisterAsset(DataID id, ScriptableObject asset)
	{
		_assets[id.AssetKey] = asset;
	}
	public void RegisterData(DataID id, object data)
	{
		_datas[id.Value] = data;
	}

	// --- Get ---
	public T GetAsset<T>(DataID id) where T : ScriptableObject
	{
		_assets.TryGetValue(id.AssetKey, out var asset);
        return asset as T;
	}
	public T GetData<T>(DataID id) where T : class
	{
		_datas.TryGetValue(id.Value, out var data);
        return data as T;
	}
}