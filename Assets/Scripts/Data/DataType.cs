public enum DataType : long
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

public struct DataIDStructure
{
	public const long max = 07777777777L; // 1,073,741,823
	public const long indent_asset = 010000000L; // 2,097,152
	public const long indent_type = 010000L; // 4,096
}