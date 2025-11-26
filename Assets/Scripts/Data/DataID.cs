using System;

/*	Data ID Structure

(4 byte)
Data ID : aaa ttt iiii

aaa : asset type
ttt  : data type
iiii : instance number
*/
public struct IDStructure
{
	public const int indent_a = 10000000;
	public const int indent_c = 10000;

	public const int indent_s = indent_a;
	public const int indent_e = 100000;
}

public struct DataIDStructure
{
	public const int indent_asset = 10000000;
	public const int indent_type = 10000;
}

public struct DataID : IEquatable<DataID>, IEquatable<int>
{
	public int value; // unsigned int
	public DataID(int value=-1) => this.value = value;

	// Property
	public readonly bool IsValid => value > 0;
	public readonly bool IsAsset => value % DataIDStructure.indent_asset > 0;
	public readonly bool IsInstance => value % DataIDStructure.indent_asset > 0;

	public readonly DataType GetAssetType => (DataType)(value / DataIDStructure.indent_asset);
	public readonly DataType GetDataType => (DataType)(value / DataIDStructure.indent_type);
	
	// public readonly Asset GetAsset => ;
	public readonly InstanceData GetData => InstanceDataManager.Get(this);

	public readonly bool Equals(int other) => value == other;
	public readonly bool Equals(DataID other) => value == other.value;
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
	Room = Organization + 2 * DataIDStructure.indent_type,

	// ----- ----- ----- Synergy ----- ----- -----
	Synergy = 4 * DataIDStructure.indent_asset,

	Stat = Synergy + 1 * DataIDStructure.indent_type,
	Ability = Synergy + 2 * DataIDStructure.indent_type,
	Disposition = Synergy + 3 * DataIDStructure.indent_type,
	Job = Synergy + 4 * DataIDStructure.indent_type,
	Skill = Synergy + 5 * DataIDStructure.indent_type,

	// ----- ----- ----- Room ----- ----- -----
}