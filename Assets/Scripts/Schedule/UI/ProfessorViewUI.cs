using TMPro;
using UnityEngine;

public class ProfessorViewUI : IGenericInventoryItem
{
	[SerializeField] private TextMeshProUGUI _title;
	[SerializeField] private TextMeshProUGUI _som;

	public override void SetData(object data) => SetData(data as ProfessorInstanceData);
	private void SetData(ProfessorInstanceData data)
	{
		_title.text = data.Asset.DisplayName;
	}
}