using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [Tooltip("카메라가 따라갈 타겟 오브젝트 (캐릭터)")]
    public Transform target;
    [Tooltip("카메라 이동의 부드러움 정도")]
    public float smoothSpeed = 0.125f;
    [Tooltip("타겟으로부터의 카메라 오프셋 (X, Y, Z)")]
    public Vector3 offset = new Vector3(0, 0, -10); // 기본 Z 오프셋을 -10으로 설정

    private void FixedUpdate()
    {
        if (target == null)
        {
            return; // 타겟이 없으면 아무것도 하지 않음
        }

        // 타겟의 위치에 오프셋을 더한 목표 위치 계산
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, 0);
        Vector3 desiredPosition = targetPosition + offset;

        // 현재 카메라 위치와 목표 위치 사이를 부드럽게 보간
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}