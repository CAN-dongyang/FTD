
using UnityEngine;

public class UI_HUD : MonoBehaviour
{
    [Header("UI Controllers")]
    public TimeDateUI timeDateUI;
    public HotbarUI hotbarUI;
    public MinimapUI minimapUI;

    // HUD 전체를 켜고 끄는 함수
    public void SetHUDVisibility(bool isVisible)
    {
        if (timeDateUI != null) timeDateUI.gameObject.SetActive(isVisible);
        if (hotbarUI != null) hotbarUI.gameObject.SetActive(isVisible);
        if (minimapUI != null) minimapUI.gameObject.SetActive(isVisible);
    }

    void Start()
    {
        SetHUDVisibility(true);
    }

    void Update()
    {
        }
}
