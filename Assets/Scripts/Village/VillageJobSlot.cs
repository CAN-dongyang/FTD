using System;

[Serializable]
public class VillageJobSlot
{
	public JobAsset job;
	public GraduateInstance worker;

	public bool IsOccupied => worker != null;

	public bool CanHire(GraduateInstance graduate)
	{
		
		return false;
	}
}