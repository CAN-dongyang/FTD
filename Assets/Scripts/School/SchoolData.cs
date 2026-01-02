using System.Collections.Generic;
using UnityEngine;

public class SchoolData : ScriptableObject
{
	public List<DataID> Students;
	public List<DataID> HiredProfessors;

	public static SchoolData Instance { get; private set; }
	public void Initialize()
	{
		Instance = this;
	}
	public void Release()
	{
		if(Instance == this) Instance = null;
	}
}