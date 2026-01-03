using UnityEngine;
using UnityEngine.UI; // Button을 사용하기 위해 추가

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    public GameObject inventorySlotPrefab; // 슬롯 프리팹을 연결할 변수 추가

    // 각 뷰(패널)들을 연결할 변수
    public GameObject inventoryView;
    public GameObject characterInfoView;
    public GameObject studentListView;
    public GameObject professorListView;
    public GameObject settingsView;

    // 각 탭 버튼들을 연결할 변수
    public Button inventoryButton;
    public Button characterInfoButton;
    public Button studentListButton;
    public Button professorListButton;
    public Button settingsButton;

    Inventory inventory;
    InventorySlot[] slots;

    protected void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        // 1. 기존에 있던 슬롯들을 모두 삭제 (에디터에 남아있는 경우를 대비)
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // 2. 인벤토리 공간(space)만큼 슬롯 프리팹을 생성
        for (int i = 0; i < inventory.space; i++)
        {
            Instantiate(inventorySlotPrefab, itemsParent);
        }

        // 버튼 리스너 등록
        inventoryButton.onClick.AddListener(() => SwitchView(inventoryView));
        characterInfoButton.onClick.AddListener(() => SwitchView(characterInfoView));
        studentListButton.onClick.AddListener(() => SwitchView(studentListView));
        professorListButton.onClick.AddListener(() => SwitchView(professorListView));
        settingsButton.onClick.AddListener(() => SwitchView(settingsView));

        // 기본 뷰 설정
        SwitchView(inventoryView);

        // 이제 동적으로 생성된 30개의 슬롯을 찾아 배열에 할당
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        // 모든 초기화가 끝난 후 인벤토리 UI를 비활성화
        inventoryUI.SetActive(false);
    }

    void Update()
    {
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    // 뷰를 전환하는 함수
    void SwitchView(GameObject viewToShow)
    {
        // 모든 뷰를 일단 비활성화
        inventoryView.SetActive(false);
        characterInfoView.SetActive(false);
        studentListView.SetActive(false);
        professorListView.SetActive(false);
        settingsView.SetActive(false);

        // 선택된 뷰만 활성화
        if (viewToShow != null)
        {
            viewToShow.SetActive(true);
        }
    }
}