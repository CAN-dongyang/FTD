using UnityEngine;
using UnityEngine.InputSystem; // Input System 네임스페이스 추가

public class MerchantNPC : MonoBehaviour
{
    [Header("Merchant Data")]
    public MerchantData merchantData; // 상점 데이터 연결

    [Header("Interaction Settings")]
    public float interactionRange = 3.0f; // 상호작용 가능 거리
    // public KeyCode interactKey = KeyCode.E; // 구형 Input System 키 (제거)

    private Transform playerTransform;
    private bool isPlayerInRange = false;

    private void Start()
    {
        // 태그로 플레이어 찾기 (Player 태그가 설정되어 있다고 가정)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // 거리 계산
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        isPlayerInRange = distance <= interactionRange;

        // 상호작용 입력 감지 (New Input System)
        // 키보드가 연결되어 있고 E키를 눌렀을 때
        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenShop();
        }
    }

    private void OpenShop()
    {
        if (merchantData == null)
        {
            Debug.LogError("MerchantData is missing!");
            return;
        }

        Debug.Log($"Opening shop for: {merchantData.merchantName}");

        // 상점 UI 열기
        if (ShopUI.Instance != null)
        {
            ShopUI.Instance.Open(merchantData);
        }
        else
        {
            Debug.LogError("ShopUI Instance is missing in the scene!");
        }
    }

    // 에디터에서 범위를 시각적으로 보여줌
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
