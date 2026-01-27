using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Asset/Skill")]
public class SkillAsset : ScriptableObject
{
	[SerializeField] private string _name;
	[SerializeField] private string _description;
	[SerializeField] private Sprite _icon;
}