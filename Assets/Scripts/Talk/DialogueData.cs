using System;
using System.Collections.Generic;
using UnityEngine;

// [변경] ScriptableObject는 데이터 저장용 껍데기로 사용하고, 
// 실제 런타임 데이터는 DialogueManager의 Dictionary를 사용하므로 간소화해도 됩니다.
public class DialogueData : ScriptableObject
{
    public string id; 
    public List<DialogueNode> nodes;
}

[Serializable]
public class DialogueChoice
{
    public string choiceText; // 선택지 텍스트 (예: "좋아!")
    public int nextID;        // 선택 시 이동할 대화 ID
}

[Serializable]
public class DialogueNode
{
    // [기존 유지] 조건부 확인
    public string conditionKey; 
    public bool conditionValue;
    
    [TextArea(3, 5)]
    public string text;

    // [추가] 다음 대화 ID (선택지가 없을 때 이어지는 대화, -1이면 대화 종료)
    public int nextID = -1; 

    // [수정] 주석 해제 및 리스트 사용
    public List<DialogueChoice> choices = new List<DialogueChoice>();

    // 조건 체크 함수 (기존 로직 유지)
    static public DialogueNode GetValidNode(List<DialogueNode> nodes, Dictionary<string, bool> currentConditions)
    {
        foreach (var node in nodes)
        {
            if (string.IsNullOrEmpty(node.conditionKey)) return node;
            if (currentConditions.TryGetValue(node.conditionKey, out bool value) && value == node.conditionValue)
                return node;
        }
        return null;
    }
}