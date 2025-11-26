using UnityEngine;

[CreateAssetMenu(fileName = "Skill_newSkill", menuName = "Synergy/Skill")]
public class SkillProperty : SynergyProperty
{
	public override SynergyType GetSynergyType => SynergyType.Skill;
}