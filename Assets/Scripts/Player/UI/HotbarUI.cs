using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarUI : MonoBehaviour
{
    public HotbarSlot[] slots; // 핫바 슬롯들
    public Color selectedColor = Color.green; // 선택되었을 때의 테두리 색상

    private Inventory inventory;
    private int selectedIndex = 0;

    void OnEnable()
    {
        InputSystem.actions.Enable(); // Player 액션 맵 활성화
        
        // 각 핫바 액션이 performed 될 때 SelectSlot 메서드를 호출하도록 이벤트 구독
        InputSystem.actions.FindAction("Hotbar1").performed += ctx => SelectSlot(0);
        InputSystem.actions.FindAction("Hotbar2").performed += ctx => SelectSlot(1);
        InputSystem.actions.FindAction("Hotbar3").performed += ctx => SelectSlot(2);
        InputSystem.actions.FindAction("Hotbar4").performed += ctx => SelectSlot(3);
        InputSystem.actions.FindAction("Hotbar5").performed += ctx => SelectSlot(4);
        InputSystem.actions.FindAction("Hotbar6").performed += ctx => SelectSlot(5);
        InputSystem.actions.FindAction("Hotbar7").performed += ctx => SelectSlot(6);
        InputSystem.actions.FindAction("Hotbar8").performed += ctx => SelectSlot(7);
        InputSystem.actions.FindAction("Hotbar9").performed += ctx => SelectSlot(8);
        InputSystem.actions.FindAction("Hotbar10").performed += ctx => SelectSlot(9);
    }

    void OnDisable()
    {
        // Player 액션 맵 비활성화 및 이벤트 구독 해제
        InputSystem.actions.FindAction("Hotbar1").performed -= ctx => SelectSlot(0);
        InputSystem.actions.FindAction("Hotbar2").performed -= ctx => SelectSlot(1);
        InputSystem.actions.FindAction("Hotbar3").performed -= ctx => SelectSlot(2);
        InputSystem.actions.FindAction("Hotbar4").performed -= ctx => SelectSlot(3);
        InputSystem.actions.FindAction("Hotbar5").performed -= ctx => SelectSlot(4);
        InputSystem.actions.FindAction("Hotbar6").performed -= ctx => SelectSlot(5);
        InputSystem.actions.FindAction("Hotbar7").performed -= ctx => SelectSlot(6);
        InputSystem.actions.FindAction("Hotbar8").performed -= ctx => SelectSlot(7);
        InputSystem.actions.FindAction("Hotbar9").performed -= ctx => SelectSlot(8);
        InputSystem.actions.FindAction("Hotbar10").performed -= ctx => SelectSlot(9);
    }

    void Start()
    {
        inventory = Inventory.instance;
        // 인벤토리 아이템이 변경될 때마다 UI를 업데이트하도록 콜백을 등록
        inventory.onItemChangedCallback += UpdateUI;

        // 초기 UI 업데이트 및 선택 슬롯 표시
        UpdateUI();
        UpdateSelection();
    }

    // 키 입력에 따라 호출될 메서드
    private void SelectSlot(int index)
    {
        selectedIndex = index;
        UpdateSelection();
    }

    // 인벤토리 데이터가 변경될 때 호출되어 핫바 UI를 새로고침
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

    // 선택된 슬롯의 테두리 색상을 업데이트
    void UpdateSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == selectedIndex)
            {
                slots[i].SetBorderColor(selectedColor);
            }
            else
            {
                slots[i].HideBorder();
            }
        }
    }
}