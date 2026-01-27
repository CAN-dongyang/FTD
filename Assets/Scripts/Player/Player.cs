using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

	[Header("Properties")]
	[SerializeField] private PlayerData _data;
	static public PlayerData Data => Instance._data;
	[SerializeField] private PlayerUIControl _playerUI;
	public static PlayerUIControl UI => Instance._playerUI;
	[SerializeField] private PlayerMove _move;
	[SerializeField] private PlayerDetect _detector;

	private bool _inputHandled;
	public bool InputHandled
	{
		get => _inputHandled;
		set
		{
			_inputHandled = value;

			if(!_inputHandled)
			{
				_move.Stop();
			}
			else if(UI.NowMenu == PlayerUIControl.MENU.LOCK)
			{
			}
		}
	}

	public Grid WorldGrid { set => _move.WorldGrid = value; }
	public Vector3Int CellPos { get => _move.CellPos; }
	public Detectable NowDetected { get => _detector.NowDetected; }

	public void Spawn(Grid grid, Vector3Int cell)
	{
		Spawn(grid.CellToWorld(cell));
	}
	public void Spawn(Vector3 pos)
	{
		_move.Body.MovePosition(pos);
	}

	#region Singleton
	private static Player _instance = null;
	public static Player Instance
	{
		get
		{
			if(!_instance) _instance = FindAnyObjectByType<Player>();
			return _instance;
		}
	}
	#endregion
}