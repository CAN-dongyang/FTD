using System;
using UnityEngine;

public interface IData
{
	public DataID ID { get; }
}

/// <summary>
/// <c>DataID</c>는 데이터 신분 번호.<br/>
/// <c>DataID</c>는 자신의 번호를 해석하여 해당 데이터를 찾을 수 있다.<br/>
/// <c>DataID</c>는 <c>DataType</c>, <c>uint</c>와 Equatable 관계다.<br/>
/// <c>DataType</c>으로 생성 가능하다.
/// 
/// <para>aa ii tt ii(hex)<br/>
/// </para>
/// </summary>
/// 
/// * IEquatable을 상속받아 비교 연산 시에 GC가 발생하지 않는다.
[Serializable]
public struct DataID : IEquatable<DataID>, IEquatable<uint>
{
	[SerializeField] public uint Value;
	public DataID(uint value) => Value = value;
	public DataID(Enum assetType, byte assetIdx, byte dataType, byte dataIdx)
	{
		uint at = (uint)Convert.ToByte(assetType) << 0x18;
		uint ai = (uint)assetIdx << 16;
		uint dt = (uint)Convert.ToByte(dataType) << 0x8;
		uint di = (uint)dataIdx;

		Value = at | ai | dt | di;
	}

	// Property
	public byte AT => (byte)((Value >> 0x18) & 0xFF);
	public byte AI => (byte)((Value >> 0x10) & 0xFF);
	public byte DT => (byte)((Value >> 0x8) & 0xFF);
	public byte DI => (byte)(Value & 0xFF);

	public uint AssetKey => Value & 0xFFFF0000;
	public uint DataKey => Value & 0x0000FFFF;

	// IEquatable
	public bool Equals(DataID other) => Value == other.Value;
	public bool Equals(uint other) => Value == other;

	public override bool Equals(object obj)
	{
		if(obj is DataID other) return Equals(other);
		if(obj is uint v) return Equals(v);
		return base.Equals(obj);
	}
	public override int GetHashCode() => (int)Value;
	public override string ToString() => $"0x{Value:X8} (AT:{AT}, AI:{AI}, DT:{DT}, DI:{DI})";
	
	public static bool operator==(DataID left, DataID right) => left.Equals(right);
	public static bool operator!=(DataID left, DataID right) => !left.Equals(right);
}

#if UNITY_EDITOR
// Inspector 제어용 Attribute
public class DataIDAttribute : PropertyAttribute
{
	public bool IsAssetOnly;
	public DataIDAttribute(bool isAssetOnly = false) => IsAssetOnly = isAssetOnly;
}
#endif