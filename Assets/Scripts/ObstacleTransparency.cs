using UnityEngine;
using System.Collections.Generic;

public class ObstacleTransparency : MonoBehaviour
{
    [Header("Settings")]
    public Transform player;
    public LayerMask obstacleLayer;
    [Range(0f, 1f)] public float transparentAlpha = 0.3f;
    public float fadeDuration = 0.5f;
    public float detectionRadius = 0.5f;
    public bool fadeGroup = true;

    // 상태 관리용
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    private Dictionary<Renderer, Coroutine> runningCoroutines = new Dictionary<Renderer, Coroutine>();
    private HashSet<Renderer> currentTransparentObjects = new HashSet<Renderer>();

    void LateUpdate()
    {
        if (player == null) return;

        HashSet<Renderer> detectedRenderers = new HashSet<Renderer>();
        HashSet<int> processedParents = new HashSet<int>();

        // 플레이어 주변 장애물 감지
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.position, detectionRadius, obstacleLayer);

        foreach (Collider2D col in colliders)
        {
            // 그룹 페이드 처리
            if (fadeGroup && col.transform.parent != null)
            {
                int parentId = col.transform.parent.GetInstanceID();
                if (processedParents.Add(parentId)) // HashSet.Add는 중복 추가 시 false 반환
                {
                    Renderer[] groupRenderers = col.transform.parent.GetComponentsInChildren<Renderer>();
                    foreach (Renderer r in groupRenderers) detectedRenderers.Add(r);
                }
            }
            else // 단일 객체 처리
            {
                Renderer r = col.GetComponent<Renderer>();
                // Renderer가 콜라이더와 다른 계층에 있을 경우 대비
                if (r == null) r = col.GetComponentInParent<Renderer>();
                if (r == null) r = col.GetComponentInChildren<Renderer>();
                
                if (r != null) detectedRenderers.Add(r);
            }
        }

        // 1. 투명화 처리 (새로 감지된 객체)
        foreach (Renderer r in detectedRenderers)
        {
            if (currentTransparentObjects.Add(r)) // 리스트에 없으면 추가하고 투명화 시작
            {
                SetTransparency(r, true);
            }
        }

        // 2. 불투명화 처리 (더 이상 감지되지 않는 객체)
        List<Renderer> toRemove = new List<Renderer>();
        foreach (Renderer r in currentTransparentObjects)
        {
            if (!detectedRenderers.Contains(r))
            {
                SetTransparency(r, false);
                toRemove.Add(r);
            }
        }

        foreach (Renderer r in toRemove)
        {
            currentTransparentObjects.Remove(r);
        }
    }

    private void SetTransparency(Renderer renderer, bool isTransparent)
    {
        // 실행 중인 코루틴 중단
        if (runningCoroutines.TryGetValue(renderer, out Coroutine runningCoroutine))
        {
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            runningCoroutines.Remove(renderer);
        }

        // 새 코루틴 시작
        Coroutine newCoroutine = StartCoroutine(FadeRoutine(renderer, isTransparent));
        runningCoroutines[renderer] = newCoroutine;
    }

    private System.Collections.IEnumerator FadeRoutine(Renderer renderer, bool isTransparent)
    {
        // 원본 머티리얼 백업 (최초 1회)
        if (!originalMaterials.ContainsKey(renderer))
        {
            originalMaterials[renderer] = new Material(renderer.material);
        }

        Material mat = renderer.material;
        Color startColor = mat.color;
        
        // 목표 색상 계산
        Color baseColor = originalMaterials[renderer].color;
        Color targetColor = isTransparent 
            ? new Color(baseColor.r, baseColor.g, baseColor.b, transparentAlpha) 
            : baseColor;

        // 투명 모드라면 렌더 모드 변경
        if (isTransparent) SetupMaterial(mat, true);

        // 페이드 애니메이션
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            mat.color = Color.Lerp(startColor, targetColor, elapsed / fadeDuration);
            yield return null;
        }
        mat.color = targetColor;

        // 불투명 모드로 돌아왔다면 렌더 모드 복구
        if (!isTransparent) SetupMaterial(mat, false);

        runningCoroutines.Remove(renderer);
    }

    private void SetupMaterial(Material mat, bool isTransparent)
    {
        if (isTransparent) // Fade Mode
        {
            mat.SetFloat("_Mode", 2);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
        else // Opaque Mode
        {
            mat.SetFloat("_Mode", 0);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.renderQueue = -1;
        }
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, detectionRadius);
        }
    }
}