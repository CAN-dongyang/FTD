using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class ShopUISetup : EditorWindow
{
    [MenuItem("Tools/Setup Shop UI")]
    public static void CreateShopUI()
    {
        // 1. Canvas 확인 또는 생성
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // 2. Shop UI Root 생성
        GameObject shopRoot = new GameObject("ShopUI", typeof(RectTransform));
        shopRoot.transform.SetParent(canvas.transform, false);
        RectTransform shopRT = shopRoot.GetComponent<RectTransform>();
        shopRT.anchorMin = Vector2.zero;
        shopRT.anchorMax = Vector2.one;
        shopRT.sizeDelta = Vector2.zero;

        // 배경 패널
        GameObject bg = new GameObject("Background", typeof(RectTransform), typeof(Image));
        bg.transform.SetParent(shopRoot.transform, false);
        bg.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
        bg.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 500);

        // 3. ShopUI 컴포넌트 추가
        ShopUI shopUI = shopRoot.AddComponent<ShopUI>();
        shopUI.shopPanel = shopRoot;

        // 4. 레이아웃 구성 (좌: 상인, 우: 플레이어)
        GameObject contentRoot = new GameObject("Content", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        contentRoot.transform.SetParent(bg.transform, false);
        RectTransform contentRT = contentRoot.GetComponent<RectTransform>();
        contentRT.sizeDelta = new Vector2(750, 400);
        
        HorizontalLayoutGroup hlg = contentRoot.GetComponent<HorizontalLayoutGroup>();
        hlg.spacing = 20;
        hlg.childControlHeight = hlg.childControlWidth = true;
        hlg.childForceExpandHeight = hlg.childForceExpandWidth = true;

        // 상인 구역
        GameObject merchantArea = CreateArea(contentRoot.transform, "MerchantInventory");
        shopUI.merchantItemParent = merchantArea.transform;

        // 플레이어 구역
        GameObject playerArea = CreateArea(contentRoot.transform, "PlayerInventory");
        shopUI.playerItemParent = playerArea.transform;

        // 5. 소지금 텍스트
        GameObject moneyObj = new GameObject("MoneyText", typeof(RectTransform), typeof(TextMeshProUGUI));
        moneyObj.transform.SetParent(bg.transform, false);
        RectTransform moneyRT = moneyObj.GetComponent<RectTransform>();
        moneyRT.anchoredPosition = new Vector2(0, -220);
        TextMeshProUGUI moneyText = moneyObj.GetComponent<TextMeshProUGUI>();
        moneyText.text = "Money: 1000";
        moneyText.alignment = TextAlignmentOptions.Center;
        shopUI.moneyText = moneyText;

        // 6. 닫기 버튼
        GameObject closeBtnObj = new("CloseButton", typeof(RectTransform), typeof(Image), typeof(Button));
        closeBtnObj.transform.SetParent(bg.transform, false);
        closeBtnObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(370, 220);
        closeBtnObj.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        closeBtnObj.GetComponent<Image>().color = Color.red;
        closeBtnObj.GetComponent<Button>().onClick.AddListener(shopUI.Close);

        Selection.activeGameObject = shopRoot;
        Debug.Log("Shop UI structure created successfully!");
    }

    private static GameObject CreateArea(Transform parent, string name)
    {
        GameObject area = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(GridLayoutGroup));
        area.transform.SetParent(parent, false);
        area.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        
        GridLayoutGroup glg = area.GetComponent<GridLayoutGroup>();
        glg.cellSize = new Vector2(100, 120);
        glg.spacing = new Vector2(10, 10);
        glg.padding = new RectOffset(10, 10, 10, 10);
        
        return area;
    }
}
