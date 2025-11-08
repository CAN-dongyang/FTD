/*	ID Structure

(4 byte)
Entity Assets ID	: aaa ccc iiii

a : asset type
c : category
i : index

(4 byte)
Instance Data ID	: sss ee iiiii

s : sub type
e : empty now
i : index
*/
public struct IDStructure
{
	public const int indent_a = 10000000;
	public const int indent_c = 10000;

	public const int indent_s = indent_a;
	public const int indent_e = 100000;
}

public enum EntityAssetType
{
	None = 0,

	// ----- ----- ----- Activity ----- ----- -----
	Activity = 1 * IDStructure.indent_a,

	Lesson = Activity + 1 * IDStructure.indent_c,
	Work = Activity + 2 * IDStructure.indent_c,

	// ----- ----- ----- Character ----- ----- -----
	Character = 2 * IDStructure.indent_a,

	// ----- ----- ----- Organization ----- ----- -----
	Organization = 3 * IDStructure.indent_a,

	// ----- ----- ----- Synergy ----- ----- -----
	Synergy = 4 * IDStructure.indent_a,

	Stat = Synergy + 1 * IDStructure.indent_c,
	Ability = Synergy + 2 * IDStructure.indent_c,
	Disposition = Synergy + 3 * IDStructure.indent_c,
	Job = Synergy + 4 * IDStructure.indent_c,
	Skill = Synergy + 5 * IDStructure.indent_c,

	// ----- ----- ----- Room ----- ----- -----

	// ----- ----- ----- End ----- ----- -----
}

public enum InstanceDataType
{
	None = 0,

	// ----- ----- ----- Activity ----- ----- -----
	Lesson = 1 * IDStructure.indent_s,
	Work = 2 * IDStructure.indent_s,

	// ----- ----- ----- Character ----- ----- -----
	Student = 3 * IDStructure.indent_s,
	Professor = 4 * IDStructure.indent_s,
	Worker = 5 * IDStructure.indent_s,

	// ----- ----- ----- Organization ----- ----- -----
	Organization = 6 * IDStructure.indent_s

	// ----- ----- ----- Synergy ----- ----- -----

	// ----- ----- ----- Room ----- ----- -----

	// ----- ----- ----- End ----- ----- -----
}