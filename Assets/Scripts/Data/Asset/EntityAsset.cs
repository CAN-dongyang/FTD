using UnityEngine;

// Entity의 원형 데이터 에셋. constant 틀
public abstract class EntityAsset : ScriptableObject
{
	#region Property
	[Header("Entity")]
	// Editor에서 특별한 커맨드를 통해 id를 일괄 부여하기 위해 수정이 불가하게 만듦
	[SerializeField] [HideInInspector] private int _id;
	public int ID => _id;

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