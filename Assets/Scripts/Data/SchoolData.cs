using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Singleton Data. Instance 프로퍼티로 불러올 수 있다.
/// </summary>
public class SchoolData : OrganizationInstanceData
{
	public List<ProfessorInstanceData> Professors
		=> member_ids.ConvertAll(id => id.GetData as ProfessorInstanceData);
	public List<LessonInstanceData> Lessons
		=> activity_ids.ConvertAll(id => id.GetData as LessonInstanceData);
	
	#region Singleton
	public static SchoolData Instance
	{
		get
		{
			return InstanceDataManager.GetAllData()
				.Find(d => d.ID.GetDataType == DataType.School) as SchoolData;
		}
	}
	#endregion

	public SchoolData(DataAsset asset) : base(asset, DataType.School) {}
}