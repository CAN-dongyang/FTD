using TMPro;

public class LessonBriefView : IGenericInventoryItem
{
	public TextMeshProUGUI lessonName;
	public TextMeshProUGUI professorName;
	public TextMeshProUGUI roomName;

	public override void SetData(object data) => SetData(data as LessonInstanceData);
	private void SetData(LessonInstanceData data)
	{
		lessonName.text = data.Asset.DisplayName;
		roomName.text = data.room.GetData is null ? "no room" : data.room.GetData.Asset.DisplayName;
		professorName.text = data.professor.GetData is null ? "no professor" : data.professor.GetData.Asset.DisplayName;
	}
}