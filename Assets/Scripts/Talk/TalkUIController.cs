using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TalkUIController : MonoBehaviour
{
    public static TalkUIController Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject talkPanel;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI nameText;
    public Button nextButton;       // 다음 대화로 넘기는 투명 버튼 (패널 전체)

    [Header("Choices")]
    public GameObject choicePanel;      // 선택지 버튼들이 들어갈 부모 오브젝트 (Vertical Layout Group 권장)
    public GameObject choiceButtonPrefab; // 버튼 프리팹

    private DialogueNode currentNode;
    private List<GameObject> activeButtons = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        
        CloseTalk();
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    // 대화 시작 진입점
    public void ShowTalk(int talkID, string npcName)
    {
        ProcessNode(talkID, npcName);
    }

    // 내부 로직 분리 (대화 진행 중 재귀 호출을 위해)
    private void ProcessNode(int talkID, string npcName = "")
    {
        // 대화 종료 조건
        if (talkID == -1)
        {
            CloseTalk();
            return;
        }

        DialogueNode node = DialogueManager.Instance.GetDialogue(talkID);
        if (node == null)
        {
            Debug.LogWarning($"대화 ID {talkID}를 찾을 수 없습니다.");
            CloseTalk();
            return;
        }

        currentNode = node;
        talkPanel.SetActive(true);
        if (!string.IsNullOrEmpty(npcName)) nameText.text = npcName;
        talkText.text = node.text;

        // 선택지 처리
        ClearChoices();
        if (node.choices != null && node.choices.Count > 0)
        {
            // 선택지가 있으면 '다음' 버튼 비활성화 (선택 강제)
            nextButton.interactable = false; 
            choicePanel.SetActive(true);

            foreach (var choice in node.choices)
            {
                GameObject btnObj = Instantiate(choiceButtonPrefab, choicePanel.transform);
                activeButtons.Add(btnObj);
                
                // 버튼 텍스트 설정 (TextMeshProUGUI가 자식에 있다고 가정)
                btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;
                
                // 버튼 클릭 리스너 연결
                Button btn = btnObj.GetComponent<Button>();
                int targetID = choice.nextID; // 클로저 캡처 주의
                btn.onClick.AddListener(() => OnChoiceSelected(targetID, npcName));
            }
        }
        else
        {
            // 선택지가 없으면 '다음' 버튼 활성화 (클릭 시 nextID로 이동)
            nextButton.interactable = true;
            choicePanel.SetActive(false);
        }
    }

    private void OnNextButtonClicked()
    {
        if (currentNode != null)
        {
            // 다음 대화로 이동
            ProcessNode(currentNode.nextID, nameText.text);
        }
    }

    private void OnChoiceSelected(int nextID, string npcName)
    {
        // 선택지 선택 시 해당 ID로 이동
        ProcessNode(nextID, npcName);
    }

    private void ClearChoices()
    {
        foreach (var btn in activeButtons) Destroy(btn);
        activeButtons.Clear();
    }

    public void CloseTalk()
    {
        talkPanel.SetActive(false);
        ClearChoices();
    }
}