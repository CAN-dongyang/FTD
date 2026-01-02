using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject obj = collision.gameObject.GetComponent<GameObject>();
		Player.Instance.detectedObject = obj;
	}
}