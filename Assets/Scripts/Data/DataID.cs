using System;
using UnityEngine;

/*	Data ID Structure

Data ID : aaa ttt iiii

aaa : asset type
ttt  : data type
iiii : instance number
*/

[Serializable]
public struct DataID : IEquatable<DataID>, IEquatable<long>
{
	[SerializeField] private long value;
	public DataID(long value=0) => this.value = value;
	public DataID(DataType type) => value = (long)type;

	// Property
	public readonly bool IsValid => value > 0;
	public readonly bool IsAsset => IsValid && !IsInstance;
	public readonly bool IsInstance => IsValid && value % DataIDStructure.indent_asset > 0;

	public readonly long GetAssetValue => value / DataIDStructure.indent_asset * DataIDStructure.indent_asset;
	public readonly long GetTypeValue => value % DataIDStructure.indent_asset / DataIDStructure.indent_type * DataIDStructure.indent_type;
	public readonly long GetInstanceValue => value % DataIDStructure.indent_type;

	public readonly DataType GetAssetType => (DataType)GetAssetValue;
	public readonly DataType GetDataType => (DataType)GetTypeValue;

	public readonly bool Equals(long other) => value == other;
	public readonly bool Equals(DataID other) => value == other.value;

	public static implicit operator long(DataID id) => id.value;
	public static implicit operator DataID(long v) => (DataID)v;
	public static implicit operator DataType(DataID id) => (DataType)id.value;
	public static implicit operator DataID(DataType t) => (DataID)t;
}