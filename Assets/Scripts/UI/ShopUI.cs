using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    [Header("UI Panels")]
    public GameObject shopPanel;
    public Transform merchantItemParent;
    public Transform playerItemParent;
    public GameObject shopSlotPrefab;
    
    [Header("Player Info")]
    public PlayerData playerData; // ScriptableObject에서 돈 정보를 가져옴
    public TextMeshProUGUI moneyText;

    private MerchantData currentMerchant;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    public void Open(MerchantData merchantData)
    {
        currentMerchant = merchantData;
        shopPanel.SetActive(true);
        
        // 게임 일시 정지 및 마우스 커서 활성화
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateUI();
    }

    public void Close()
    {
        shopPanel.SetActive(false);
        
        // 게임 재개
        Time.timeScale = 1f;
        // Cursor.lockState = CursorLockMode.Locked; // 필요 시 주석 해제
    }

    public void UpdateUI()
    {
        // 기존 슬롯 제거
        foreach (Transform child in merchantItemParent) Destroy(child.gameObject);
        foreach (Transform child in playerItemParent) Destroy(child.gameObject);

        // 상인 물품 목록 생성
        foreach (Item item in currentMerchant.sellItems)
        {
            GameObject obj = Instantiate(shopSlotPrefab, merchantItemParent);
            obj.GetComponent<ShopSlot>().Setup(item, true); // true = 구매 모드
        }

        // 플레이어 인벤토리 목록 생성
        foreach (Item item in playerData.Inventory.items)
        {
            GameObject obj = Instantiate(shopSlotPrefab, playerItemParent);
            obj.GetComponent<ShopSlot>().Setup(item, false); // false = 판매 모드
        }

        // 소지금 갱신
        if (moneyText != null && playerData != null)
            moneyText.text = $"Money: {playerData.money}";
    }

    public void BuyItem(Item item)
    {
        if (playerData.money >= item.price)
        {
            if (playerData.Inventory.Add(item))
            {
                playerData.money -= item.price;
                UpdateUI();
            }
            else
            {
                Debug.Log("인벤토리가 가득 찼습니다.");
            }
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }
    }

    public void SellItem(Item item)
    {
        playerData.money += item.price;
        playerData.Inventory.Remove(item);
        UpdateUI();
    }
}
