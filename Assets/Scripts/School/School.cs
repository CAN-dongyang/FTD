using UnityEngine;

public class School : MonoBehaviour
{
	[Tooltip("이 Object가 School Data의 싱글톤을 담당")]
	[SerializeField] private SchoolData _data;
	[SerializeField] private Grid _schoolGrid;

	static public SchoolData Data => Instance._data;
	public Grid Grid => _schoolGrid;

	#region Singleton
	private static School _instance = null;
	public static School Instance
	{
		get
		{
			if(!_instance) _instance = FindAnyObjectByType<School>();
			return _instance;
		}
	}
	#endregion
}