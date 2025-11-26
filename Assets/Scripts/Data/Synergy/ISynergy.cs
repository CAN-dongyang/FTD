public interface ISynergy
{
	// StatType : [0 ~ StatType.End]
	// SynergyProperty : Asset ID
	public DataID ID { get; }

	public int GetSynergyType => ID.GetCategoryValue;
}