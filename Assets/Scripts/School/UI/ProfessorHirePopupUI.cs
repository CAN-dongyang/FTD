using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.Linq;

public class ProfessorHirePopupUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI majorText;
    [SerializeField] private TextMeshProUGUI salaryText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI dispositionText;
    [SerializeField] private Button hireButton;
    [SerializeField] private Button fireButton; // 해고 버튼 추가
    [SerializeField] private Button closeButton;

    private ProfessorData _data;

    public void Open()
    {
        _data = null; // 알아 채우기

		if(hireButton) hireButton.onClick.AddListener(OnHireClicked);
        if(fireButton) fireButton.onClick.AddListener(OnFireClicked);
        if(closeButton) closeButton.onClick.AddListener(ClosePopup);
		
		UpdateUI();
		gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);

		if(hireButton) hireButton.onClick.RemoveListener(OnHireClicked);
        if(fireButton) fireButton.onClick.RemoveListener(OnFireClicked);
        if(closeButton) closeButton.onClick.RemoveListener(ClosePopup);
    }

    private void UpdateUI()
    {
        if (_data == null) return;

        if (nameText) nameText.text = _data.Name;
        if (majorText) majorText.text = $"전공: {_data.Major}";
        if (salaryText) salaryText.text = $"비용: {_data.Salary}";
        if (descriptionText) descriptionText.text = _data.Description;
        
        // 성격 표시
        if (dispositionText)
        {
            if (_data.DispositionIds != null && _data.DispositionIds.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var id in _data.DispositionIds)
                {
                    sb.Append($"성향({id}) ");
                }
                dispositionText.text = sb.ToString();
            }
            else
            {
                dispositionText.text = "성향 없음";
            }
        }

        // 현재 고용 상태에 따라 버튼 상태 설정
        bool isHired = false;//ProfessorDataManager.Instance != null && ProfessorDataManager.Instance.HiredProfessors.Any(p => p.Id == _data.Id);
        SetHiredState(isHired);
    }

    private void OnHireClicked()
    {
        if (_data != null)// && ProfessorDataManager.Instance != null)
        {
            bool success = true;//ProfessorDataManager.Instance.HireProfessor(_data.Id);
            if (success)
            {
                SetHiredState(true);

				/// update list

                ClosePopup(); // 고용 성공 시 팝업 닫기
            }
        }
    }
    private void OnFireClicked()
    {
        if (_data != null)// && ProfessorDataManager.Instance != null)
        {
            bool success = true; //ProfessorDataManager.Instance.FireProfessor(_data.Id);
            if (success)
            {
                SetHiredState(false);
                
				// update list

                ClosePopup(); // 해고 성공 시 팝업 닫기
            }
        }
    }
    private void SetHiredState(bool isHired)
    {
        if (hireButton)
        {
            hireButton.gameObject.SetActive(!isHired); // 고용된 상태가 아니면 고용 버튼 활성화
        }
        if (fireButton)
        {
            fireButton.gameObject.SetActive(isHired); // 고용된 상태면 해고 버튼 활성화
        }
    }
}
