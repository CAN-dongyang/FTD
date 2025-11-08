using UnityEngine;

[CreateAssetMenu(fileName = "Job_newJob", menuName = "Synergy/Job")]
public class JobProperty : SynergyProperty
{
	public override SynergyType GetSynergyType => SynergyType.Job;
}