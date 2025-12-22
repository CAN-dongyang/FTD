using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Data Source")]
    public TextAsset talkCsvFile; // CSV 파일 연결

    // 런타임 대화 데이터 (ID -> 대화 노드 리스트)
    private Dictionary<int, List<DialogueNode>> dialogueDatabase = new Dictionary<int, List<DialogueNode>>();

    // 게임 상태 조건 (예: "HasMet_StudentA" -> true)
    // 실제 게임에서는 별도의 GameManager나 QuestManager에서 관리해야 합니다.
    public Dictionary<string, bool> gameConditions = new Dictionary<string, bool>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        LoadCsvData();
        
        // 테스트용 초기 조건 설정 (필요 시)
        // SetCondition("FirstMeet", true);
    }

    // DialogueManager.cs의 LoadCsvData 메서드 수정

    private void LoadCsvData()
    {
        if (talkCsvFile == null) return;

        string[] lines = talkCsvFile.text.Split('\n');

        // i = 1부터 시작 (헤더 건너뛰기)
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            // 쉼표 안에 있는 쉼표(대화 내용 중) 처리가 필요하지만, 일단 간단한 Split으로 구현
            string[] parts = lines[i].Split(',');

            if (parts.Length < 2) continue;

            if (int.TryParse(parts[0], out int id))
            {
                string text = parts[1].Replace("{c}", ","); // 쉼표 이스케이프
                
                // [추가] NextID 파싱 (3번째 컬럼)
                int nextId = -1;
                if (parts.Length > 2 && int.TryParse(parts[2], out int nId)) nextId = nId;

                // [추가] Choices 파싱 (4번째 컬럼, 예: "네:100|아니오:200")
                List<DialogueChoice> choices = new List<DialogueChoice>();
                if (parts.Length > 3 && !string.IsNullOrEmpty(parts[3]))
                {
                    string[] choiceSplit = parts[3].Split('|');
                    foreach (var c in choiceSplit)
                    {
                        string[] cParts = c.Split(':');
                        if (cParts.Length == 2 && int.TryParse(cParts[1], out int targetId))
                        {
                            choices.Add(new DialogueChoice { choiceText = cParts[0], nextID = targetId });
                        }
                    }
                }

                string condKey = parts.Length > 4 ? parts[4].Trim() : "";
                bool condVal = parts.Length > 5 && bool.TryParse(parts[5], out bool res) ? res : true;

                DialogueNode node = new DialogueNode
                {
                    text = text,
                    nextID = nextId,     // [추가]
                    choices = choices,   // [추가]
                    conditionKey = condKey,
                    conditionValue = condVal
                };

                if (!dialogueDatabase.ContainsKey(id))
                    dialogueDatabase[id] = new List<DialogueNode>();
                dialogueDatabase[id].Add(node);
            }
        }
        Debug.Log($"[DialogueManager] 로드 완료: {dialogueDatabase.Count}개 그룹");
    }

    // ID에 해당하는 대화 중 현재 조건에 맞는 하나를 반환
    public DialogueNode GetDialogue(int id)
    {
        if (dialogueDatabase.ContainsKey(id))
        {
            return DialogueNode.GetValidNode(dialogueDatabase[id], gameConditions);
        }
        return null;
    }

    // 조건 설정 함수
    public void SetCondition(string key, bool value)
    {
        if (gameConditions.ContainsKey(key))
            gameConditions[key] = value;
        else
            gameConditions.Add(key, value);
    }
}