using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Acts", menuName = "Asset/Activity")]
public class ActivityAsset : EntityAsset
{
	private UnityEvent _onCompleted;

	public UnityEvent OnCompleted => _onCompleted;
}