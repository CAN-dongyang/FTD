using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ProfessorSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button actionButton; // 상세보기 버튼

    private ProfessorData _data;
    private Action<ProfessorData> _onClickCallback;
    private bool _isHired;

    // 초기화 메서드: 데이터, 클릭 콜백, 고용 여부를 받음
    public void Setup(ProfessorData data, Action<ProfessorData> onClickCallback, bool isHired)
    {
        _data = data;
        _onClickCallback = onClickCallback;
        _isHired = isHired;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_data == null) return;

        // 이름 표시
        if (nameText) nameText.text = _data.Name;

        // 버튼 상태 업데이트
        if (actionButton)
        {
            TextMeshProUGUI buttonText = actionButton.GetComponentInChildren<TextMeshProUGUI>();
            if (_isHired)
            {
                if (buttonText) buttonText.text = "이력서 보기"; // 고용 완료 상태에서도 이력서 보기 가능
                actionButton.interactable = true; // 버튼 활성화
            }
            else
            {
                if (buttonText) buttonText.text = "상세 보기";
                actionButton.interactable = true; // 고용 안 됨 -> 상세 보기 가능
            }
        }
    }

    private void Start()
    {
        if (actionButton)
        {
            actionButton.onClick.AddListener(OnClicked);
        }
    }

    private void OnClicked()
    {
        // 버튼 클릭 시 콜백 호출 (팝업 띄우기 등)
        _onClickCallback?.Invoke(_data);
    }
}
