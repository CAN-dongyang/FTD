using UnityEngine;

public class SchoolObject : MonoBehaviour, IObject
{
	[SerializeField] private Grid _schoolGrid;

	public SchoolData Data => SchoolData.Instance;

	public void Has()
	{
	}
}