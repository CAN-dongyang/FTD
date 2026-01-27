using System;

[Serializable]
public class CharacterInstance
{
	[NonSerialized] public CharacterAsset asset;
	public string studentID;

	public CharacterInstance(CharacterAsset a)
	{
		asset = a;
	}
}