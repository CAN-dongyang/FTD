using UnityEngine;
using UnityEngine.InputSystem; // Input System 네임스페이스

public class TalkInteraction : MonoBehaviour
{
    public float interactionRange = 2.0f;
    private Transform playerTransform;

    // Input Actions 자동 생성 클래스
    private InputActions inputActions; 

    private void Awake()
    {
        inputActions = new InputActions();
        playerTransform = transform;
    }

    private void OnEnable()
    {
        // [중요] 에디터에서 설정한 'Player' 맵의 'Interact' 액션을 연결합니다.
        // 키가 E로 설정되어 있다면, E를 누를 때 이 함수가 실행됩니다.
        inputActions.Player.Interact.performed += OnInteractPerformed;
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteractPerformed;
        inputActions.Player.Disable();
    }
    
    // E키가 눌렸을 때 실행되는 함수
    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("[1] 상호작용 키(E) 입력 감지."); 
        CheckForInteraction();
    }

    void CheckForInteraction()
    {
        // 1. 씬에 있는 모든 NPC 데이터(TalkTestAuthoring) 가져오기
        TalkTestAuthoring[] allTalkers = FindObjectsByType<TalkTestAuthoring>(FindObjectsSortMode.None); 
        
        Debug.Log($"[2] 씬에서 대화 가능 대상 {allTalkers.Length}개 탐색 중...");
        
        GameObject closestNPC = null;
        float closestDist = interactionRange;

        // 2. 가장 가까운 NPC 찾기
        foreach (TalkTestAuthoring talker in allTalkers)
        {
            // NPC가 아니거나 자기 자신이면 패스
            if (!talker.IsNPC || talker.gameObject.Equals(gameObject)) continue; 

            GameObject npc = talker.gameObject;
            float distance = Vector3.Distance(playerTransform.position, npc.transform.position);

            if (distance < closestDist)
            {
                closestDist = distance;
                closestNPC = npc;
            }
        }

        // 3. 결과 처리
        if (closestNPC != null)
        {
            Debug.Log($"[3] '{closestNPC.name}'와 대화 시도 (거리: {closestDist:F2})");
            
            TalkTestAuthoring authoring = closestNPC.GetComponent<TalkTestAuthoring>();

            if (authoring != null && TalkUIController.Instance != null)
            {
                // 대화창 띄우기
                TalkUIController.Instance.ShowTalk(authoring.TalkID, closestNPC.name);
            }
            else
            {
                Debug.LogError("[4] 오류: TalkUIController가 없거나 NPC 설정이 잘못되었습니다.");
            }
        }
        else
        {
            Debug.Log($"[3] 범위({interactionRange:F2}) 내에 대화할 NPC가 없습니다.");
        }
    }
}