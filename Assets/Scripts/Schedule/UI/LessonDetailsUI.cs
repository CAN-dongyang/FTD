using System.Linq;
using UnityEngine;

public class LessonDetailsUI : MonoBehaviour
{
	[Tooltip("Start에서 SetContents 해주기 위해")]
	[SerializeField] private GenericInventory _lessons;
	[SerializeField] private GenericInventory _professors;

	public void SetData(LessonBriefView data)
	{
	}

	private void Start()
	{
		_lessons.SetContents(SchoolData.Instance.activity_ids.ConvertAll<object>(l => l));
		_professors.SetContents(SchoolData.Instance.Professors.ConvertAll<object>(p => p));
	}
}