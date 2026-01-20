using UnityEngine;

public class Home : MonoBehaviour
{
	[SerializeField] private Grid _gridExternal;
	[SerializeField] private Grid _gridInternal;
	public Grid GridExternal => _gridExternal;
	public Grid GridInternal => _gridInternal;

	public void MoveIn()
	{
		if(Player.Instance) Player.Instance.Spawn(GridInternal, Vector3Int.zero);
	}
	public void MoveOut()
	{
		if(Player.Instance) Player.Instance.Spawn(GridExternal, new Vector3Int(8,23));
	}
}