using UnityEngine;

[CreateAssetMenu(fileName = "Job", menuName = "Asset/Job")]
public class JobAsset : ScriptableObject
{
	[Header("Basic Info")]
	public string displayName;
	public string description;
	public Sprite icon;

	[Header("Requirements")]
	public CharacterStatus requireStatus;

	[Header("Carrer Path")]
	public JobAsset nextTierJob;
}

/*
public enum Subject
{
    Swordsmanship, // 검술
    Magic,         // 마법
    TeaCeremony,   // 다도
    Dancing,       // 무용
    Knowledge      // 지식
}
*/