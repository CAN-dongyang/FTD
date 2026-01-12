using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI priceText;
    public Button actionButton;

    private Item currentItem;
    private bool isBuying;

    public void Setup(Item item, bool isBuyingMode)
    {
        currentItem = item;
        isBuying = isBuyingMode;

        icon.sprite = item.icon;
        priceText.text = $"{item.price}";

        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(OnSlotClicked);
    }

    private void OnSlotClicked()
    {
        if (currentItem == null) return;

        if (isBuying)
        {
            ShopUI.Instance.BuyItem(currentItem);
        }
        else
        {
            ShopUI.Instance.SellItem(currentItem);
        }
    }
}
