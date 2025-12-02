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

#if UNITY_EDITOR
	[SerializeField] private DataType _setAssetType_inEditor;
	private DataType _prevType;

	private void OnValidate()
	{
		if(_prevType != _setAssetType_inEditor)
		{
			_id = new(_setAssetType_inEditor);
			_prevType = _setAssetType_inEditor;

			UnityEditor.EditorUtility.SetDirty(this);
			UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
			UnityEditor.AssetDatabase.Refresh();

			if(!_id.IsAsset)
			{
				Debug.LogWarning($"{name}에 적용된 아이디 타입이 에셋이 아닙니다");
			}
		}
	}
#endif
	#endregion
}