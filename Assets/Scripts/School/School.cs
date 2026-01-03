using UnityEngine;

public class School : MonoBehaviour
{
	[Tooltip("이 Object가 School Data의 싱글톤을 담당")]
	[SerializeField] private SchoolData _data;
	[SerializeField] private Grid _schoolGrid;

	public SchoolData Data => _data;

	public static School Instance { get; private set; }
	private void Awake() => Instance = this;
	private void OnDestroy() { if(Instance == this) Instance = null; }
}