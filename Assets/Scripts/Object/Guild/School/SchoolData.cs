using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="School_Data", menuName="Asset/School_Data")]
public class SchoolData : ScriptableObject, IData
{
	[SerializeField] private DataID _id;
	public DataID ID => _id;
	
	[SerializeField] private List<DataID> professors;
	[SerializeField] private List<DataID> students;

	private static string _filePath = "SchoolData";
	private static SchoolData _instance = null;
	public static SchoolData Instance
	{
		get
		{
			if(!_instance)
				_instance = Resources.Load<SchoolData>(_filePath);
			return _instance;
		}
	}
}