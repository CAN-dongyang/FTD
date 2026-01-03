using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Player.Instance.Detect(collision.gameObject);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(Player.Instance.NowDetected == collision.gameObject)
			Player.Instance.Detect(null);
	}
}