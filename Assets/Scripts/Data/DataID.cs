using System;
using UnityEngine;

/*	Data ID Structure

(4 byte)
Data ID : aaa ttt iiii

aaa : asset type
ttt  : data type
iiii : instance number
*/
public struct DataIDStructure
{
	public const int indent_asset = 10000000;
	public const int indent_type = 10000;
}

[Serializable]
public struct DataID : IEquatable<DataID>, IEquatable<int>
{
	[SerializeField] private int value;
	public DataID(int value=-1) => this.value = value;
	public DataID(DataType type) => value = (int)type;

	// Property
	public readonly bool IsValid => value > 0;
	public readonly bool IsAsset => IsValid && !IsInstance;
	public readonly bool IsInstance => IsValid && value % DataIDStructure.indent_asset > 0;

	public readonly int GetAssetValue => value / DataIDStructure.indent_asset * DataIDStructure.indent_asset;
	public readonly int GetTypeValue => value % DataIDStructure.indent_asset / DataIDStructure.indent_type * DataIDStructure.indent_type;
	public readonly int GetInstanceValue => value % DataIDStructure.indent_type;

	public readonly DataType GetAssetType => (DataType)GetAssetValue;
	public readonly DataType GetDataType => (DataType)GetTypeValue;
	
	public readonly DataAsset GetAsset => InstanceDataManager.GetAsset(GetAssetValue);
	public readonly InstanceData GetData => InstanceDataManager.GetData(this);

	public readonly bool Equals(int other) => value == other;
	public readonly bool Equals(DataID other) => value == other.value;

	public static implicit operator int(DataID id) => id.value;
	public static implicit operator DataID(int v) => (DataID)v;
	public static implicit operator DataType(DataID id) => (DataType)id.value;
	public static implicit operator DataID(DataType t) => (DataID)t;
}

public enum DataType
{
	None = 0,

	// ----- ----- ----- Activity ----- ----- -----
	Activity = 1 * DataIDStructure.indent_asset,

	Lesson = Activity + 1 * DataIDStructure.indent_type,
	Work = Activity + 2 * DataIDStructure.indent_type,

	// ----- ----- ----- Character ----- ----- -----
	Character = 2 * DataIDStructure.indent_asset,

	Student = Character + 1 * DataIDStructure.indent_type,
	Professor = Character + 2 * DataIDStructure.indent_type,
	Worker = Character + 3 * DataIDStructure.indent_type,

	// ----- ----- ----- Organization ----- ----- -----
	Organization = 3 * DataIDStructure.indent_asset,

	School = Organization + 1 * DataIDStructure.indent_type,

	// ----- ----- ----- Synergy ----- ----- -----
	Synergy = 4 * DataIDStructure.indent_asset,

	Stat = Synergy + 1 * DataIDStructure.indent_type,
	Ability = Synergy + 2 * DataIDStructure.indent_type,
	Disposition = Synergy + 3 * DataIDStructure.indent_type,
	Job = Synergy + 4 * DataIDStructure.indent_type,
	Skill = Synergy + 5 * DataIDStructure.indent_type,
}