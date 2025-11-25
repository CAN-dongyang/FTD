using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("캐릭터의 이동 속도")]
    public float moveSpeed = 5f;


    private Rigidbody2D rb;
    private Vector2 moveInput;
    private InputActions inputActions;
    private void Awake()
    {
        // Rigidbody2D 컴포넌트 가져오기.
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Input Actions 초기화 및 활성화
        inputActions = new InputActions();
        inputActions.Player.Enable();

        // Move 액션에 대한 콜백 등록
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
  
    }

    private void OnDestroy()
    {
        // 스크립트 비활성화 시 Input Actions 비활성화
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
       
    }

    private void Move()
    {
        // 입력 값을 기반으로 이동 방향 벡터 생성
        Vector2 moveDirection = moveInput.normalized;

        // 다음 위치 계산
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);

    }
}
