using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDetect : MonoBehaviour
{
	[SerializeField] private LayerMask _detectLayer;

	public Detectable NowDetected { get; private set; }

	public void Detect(Detectable d)
	{
		if(NowDetected) NowDetected.OnFocusOut.Invoke();
		NowDetected = d;
		if(NowDetected) NowDetected.OnDetect.Invoke();
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if((_detectLayer & collision.includeLayers) > 0)
		{
			var detected = collision.GetComponent<Detectable>();
			if(detected) Detect(detected);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if((_detectLayer.value & collision.includeLayers.value) > 0
			&& NowDetected == collision.GetComponent<Detectable>())
		{
			Detect(null);
		}
	}

	private void GetInteractInput(InputAction.CallbackContext cb)
	{
		if(Player.Instance.InputHandled && NowDetected != null)
			NowDetected.OnInteract.Invoke();
	}
	private void OnEnable()
	{
		var act = InputSystem.actions.FindAction("Interact");
		if(act is not null) act.started += GetInteractInput;
	}
	private void OnDisable()
	{
		var act = InputSystem.actions.FindAction("Interact");
		if(act is not null) act.started -= GetInteractInput;
	}
}