using System.Collections.Generic;
using UnityEngine;

public class SchoolData : ScriptableObject
{

	[SerializeField] private List<DataID> professors;
	[SerializeField] private List<DataID> students;
}