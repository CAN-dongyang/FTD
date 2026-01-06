using UnityEngine;
using UnityEngine.Events;

public class Detectable : MonoBehaviour
{
	[SerializeField] private UnityEvent _onDetect;
	[SerializeField] private UnityEvent _onDetectOut;
	[SerializeField] private UnityEvent<bool> _onInteract;

	public UnityEvent OnDetect => _onDetect ??= new();
	public UnityEvent OnFocusOut => _onDetectOut ??= new();
	public UnityEvent<bool> OnInteract => _onInteract ??= new();
}