using UnityEngine;

public class SchoolObject : MonoBehaviour
{
	[Tooltip("이 Object가 School Data의 싱글톤을 담당")]
	[SerializeField] private SchoolData _data;
	[SerializeField] private Grid _schoolGrid;

	private void Awake() => _data.Initialize();
	private void OnDestroy() => _data.Release();
}