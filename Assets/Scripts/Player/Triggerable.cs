using UnityEngine;
using UnityEngine.Events;

public class Triggerable : MonoBehaviour
{
	[SerializeField] private UnityEvent _onDetect;
	[SerializeField] private UnityEvent _onDetectOut;
	[SerializeField] private UnityEvent<bool> _onInteract;

	public UnityEvent OnDetect => _onDetect ??= new();
	public UnityEvent OnFocusOut => _onDetectOut ??= new();
	public UnityEvent<bool> OnInteract => _onInteract ??= new();


	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			OnDetect.Invoke();
		}
	}
}