using UnityEngine;

// 원형 데이터 에셋. constant 틀
public abstract class DataAsset : ScriptableObject
{
	#region Property
	[Header("Asset")]
	[SerializeField] private DataID _id;
	public DataID ID => _id;

	[SerializeField] private string _displayName;
	public string DisplayName => _displayName;

	[TextArea]
	[SerializeField] private string _description;
	public string Desc => _description;
	#endregion
	
#if UNITY_EDITOR
	// Zero ID Notification
	private void OnValidate()
	{
		if (ID == 0) Debug.LogAssertion($"{this}의 ID가 0입니다. FTD 메뉴의 ID Generation을 실행해주세요.");
	}
#endif
}