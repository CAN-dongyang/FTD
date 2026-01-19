using UnityEngine;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
	private void FixedUpdate()
	{
		GameData.Time.Tick(Time.fixedDeltaTime);
	}
	private void Awake()
	{
		InputSystem.actions.Enable();
		GameData.Initialize();
		GameData.Time.Start();
	}
	private void OnDestroy()
	{
		InputSystem.actions.Disable();
		GameData.Release();
	}
}