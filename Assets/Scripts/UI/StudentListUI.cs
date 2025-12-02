using UnityEngine;
using TMPro;
using System.Linq;

public class StudentListUI : MonoBehaviour
{
    public Transform contentParent; // Scroll View의 Content 오브젝트를 연결할 변수
    public GameObject studentListItemPrefab; // 학생 아이템 프리팹을 연결할 변수

    void OnEnable()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        // 1. 기존에 생성된 리스트 아이템들을 모두 삭제
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

		// 2. InstanceDataManager에서 모든 CharacterData를 가져온 뒤, ID를 기준으로 학생(type 1)만 필터링
		var allStudents = InstanceDataManager.GetAllData().OfType<StudentInstanceData>();
            //.Where(studentData => EntityID.GetTypeID(studentData.Asset.ID) == 1);

        // 3. 필터링된 학생 목록을 순회하며 UI 아이템 생성
        foreach (var studentData in allStudents)
        {
            GameObject newItem = Instantiate(studentListItemPrefab, contentParent);
            
            // 프리팹에서 TextMeshProUGUI 컴포넌트를 찾아 이름 설정
            TextMeshProUGUI nameText = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (nameText != null)
            {
                // DataAsset의 DisplayName 프로퍼티를 사용.
                nameText.text = studentData.Asset.DisplayName; 
            }
        }
    }
}
