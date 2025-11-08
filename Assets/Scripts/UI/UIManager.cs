using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] uiWindows;

    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // 매 프레임마다 호출
    void Update()
    {
        if (inputActions.Player.Inventory.WasPressedThisFrame())
        {
            ToggleWindow("InventoryPanel");
        }
    }

    public void ToggleWindow(string windowName)
    {
        foreach (var window in uiWindows)
        {
            if (window.name == windowName)
            {
                window.SetActive(!window.activeSelf);
            }
        }
    }

    public void OpenWindow(string windowName)
    {
        foreach (var window in uiWindows)
        {
            if (window.name == windowName)
            {
                window.SetActive(true);
            }
        }
    }

    public void CloseWindow(string windowName)
    {
        foreach (var window in uiWindows)
        {
            if (window.name == windowName)
            {
                window.SetActive(false);
            }
        }
    }
}
