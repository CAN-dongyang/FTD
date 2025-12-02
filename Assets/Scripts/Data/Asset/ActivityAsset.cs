using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Acts", menuName = "Asset/Activity")]
public class ActivityAsset : DataAsset
{
	private UnityEvent _onCompleted;

	public UnityEvent OnCompleted => _onCompleted;
}