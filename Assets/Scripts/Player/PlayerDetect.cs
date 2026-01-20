using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
	[SerializeField] private LayerMask _detectLayer;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if((_detectLayer & collision.includeLayers) > 0)
		{
			var detected = collision.GetComponent<Detectable>();
			if(detected) Player.Instance.Detect(detected);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if((_detectLayer.value & collision.includeLayers.value) > 0
			&& Player.Instance.NowDetected == collision.GetComponent<Detectable>())
		{
			Player.Instance.Detect(null);
		}
	}
}