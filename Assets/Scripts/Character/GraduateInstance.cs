using System;

[Serializable]
public class GraduateInstance
{
	[NonSerialized] public CharacterAsset asset;

	public CharacterStatus finalStats;	// 졸업 당시 능력치
	public int affection;	// 교장과의 호감도

	public JobAsset currentJob;
	public VillageBuilding workplace;

	public GraduateInstance(CharacterInstance ch)
	{
		asset = ch.asset;
	}
}