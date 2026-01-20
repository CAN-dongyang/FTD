using Unity.Collections;
using UnityEngine;

public enum AssetType : uint
{
	Country = 1,
	Guild,
	Character,
	Item,
	Room,

	None = 255
}

public abstract class BaseAssetSO : ScriptableObject
{
	[Header("ID Settings")]
	[ReadOnly, SerializeField] private AssetType _assetType;
	[ReadOnly, SerializeField] private byte _assetIndex;

	public AssetType AssetType => _assetType;
    public byte AssetIndex => _assetIndex;

	// 레지스트리만 이 값을 수정할 수 있도록 설정
    public void SetAddress(AssetType type, byte index)
    {
        _assetType = type;
        _assetIndex = index;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}