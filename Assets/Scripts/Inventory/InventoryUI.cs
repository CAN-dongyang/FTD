using UnityEngine;
using UnityEngine.UI; // Button을 사용하기 위해 추가
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform hotbarParent; // 핫바 영역
    public Transform inventoryParent; // 인벤토리 영역

    public GameObject inventoryUI;
    public GameObject inventorySlotPrefab; // 슬롯 프리팹을 연결할 변수 추가
    public Button sortButton;

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

    private void OnEnable()
    {
        if(InputSystem.actions == null) return;
        InputSystem.actions.Enable();

        var action = InputSystem.actions.FindAction("Inventory");
        if(action != null)
        {
            action.started += OnInventoryKey;
        }
    }

    private void OnDisable()
    {
        if(InputSystem.actions == null) return;
        var action = InputSystem.actions.FindAction("Inventory");
        if(action != null)
        {
            action.started -= OnInventoryKey;
        }
    }

    private void OnInventoryKey(InputAction.CallbackContext context)
    {
        ToggleInventory();
    }

    protected void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        // 1. 기존에 있던 슬롯들을 모두 삭제 (에디터에 남아있는 경우를 대비)
        foreach (Transform child in hotbarParent) Destroy(child.gameObject);
        foreach (Transform child in inventoryParent) Destroy(child.gameObject);

        List<InventorySlot> createdSlots = new List<InventorySlot>();

        // 2. 인벤토리 공간(space)만큼 슬롯 프리팹을 생성
        for (int i = 0; i < inventory.space; i++)
        {
            Transform targetParent = (i < 10) ? hotbarParent : inventoryParent;

            GameObject slotObj = Instantiate(inventorySlotPrefab, targetParent);
            createdSlots.Add(slotObj.GetComponent<InventorySlot>());
        }

        slots = createdSlots.ToArray();

        // 정렬버튼
        if(sortButton) sortButton.onClick.AddListener(() => inventory.SortItems());

        // 버튼 리스너 등록
        inventoryButton.onClick.AddListener(() => SwitchView(inventoryView));
        characterInfoButton.onClick.AddListener(() => SwitchView(characterInfoView));
        studentListButton.onClick.AddListener(() => SwitchView(studentListView));
        professorListButton.onClick.AddListener(() => SwitchView(professorListView));
        settingsButton.onClick.AddListener(() => SwitchView(settingsView));

        // 기본 뷰 설정
        SwitchView(inventoryView);

        // 모든 초기화가 끝난 후 인벤토리 UI를 비활성화
        inventoryUI.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void ToggleInventory()
    {
        bool isActive = !inventoryUI.activeSelf;
        inventoryUI.SetActive(isActive);

        if(isActive)
        {
            Time.timeScale = 0f; // 게임 일시정지
            SwitchView(inventoryView); // 인벤토리 뷰로 전환
        }
        else
        {
            Time.timeScale = 1f; // 게임 재개
        }
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