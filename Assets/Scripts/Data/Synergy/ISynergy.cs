public enum SynergyType // 필요한 순간이 분명 올 것 같은데...
{
	Stat,
	Ability,
	Disposition,
	Job,
	Skill
}

public interface ISynergy
{
	// StatType : [0 ~ StatType.End]
	// SynergyProperty : Entity ID
	public int ID { get; }
	
	public SynergyType GetSynergyType { get; } // 필요한 순간이 분명 올 것 같은데...
}