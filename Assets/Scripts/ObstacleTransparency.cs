using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class ObstacleTransparency : MonoBehaviour
{
    [Header("Target and Layer Settings")]
    [Tooltip("카메라와 플레이어 사이에 있는 장애물을 투명하게 만들지 결정합니다.")]
    public Transform player; // 플레이어 Transform
    [Tooltip("장애물로 인식할 레이어")]
    public LayerMask obstacleLayer; // 장애물 레이어

    [Header("Transparency Settings")]
    [Tooltip("장애물이 투명해질 때의 알파 값")]
    [Range(0.0f, 1.0f)]
    public float transparentAlpha = 0.3f; // 투명해질 때의 알파 값
    [Tooltip("투명해지거나 불투명해지는 시간")]
    public float fadeDuration = 0.5f; // 페이드 시간

    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    private List<Renderer> currentlyTransparentObstacles = new List<Renderer>();
    private Dictionary<TilemapRenderer, Material> originalTilemapMaterials = new Dictionary<TilemapRenderer, Material>(); // 타일맵용 Material 저장
    private List<TilemapRenderer> currentlyTransparentTilemapObstacles = new List<TilemapRenderer>(); // 현재 투명한 타일맵 렌더러

    void LateUpdate()
    {
        Debug.Log("ObstacleTransparency script is running!"); // 스크립트 실행 확인을 위한 로그

        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned. Please assign the player in the Inspector.");
            return;
        }
        else
        {
            Debug.Log("[ObstacleTransparency] Player is assigned. Player position: " + player.position); // Player 할당 확인 로그
        }

        Vector3 cameraToPlayerDirection = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(player.position, transform.position);

        List<Renderer> newObstaclesToTransparent = new List<Renderer>();
        List<TilemapRenderer> newTilemapObstaclesToTransparent = new List<TilemapRenderer>();

        // 카메라에서 플레이어까지 2D 레이캐스트
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(new Ray(transform.position, cameraToPlayerDirection), distance, obstacleLayer);

        // 레이캐스트가 감지한 오브젝트 수를 출력
        if (hits.Length > 0)
        {
            Debug.Log($"[ObstacleTransparency] Raycast detected {hits.Length} obstacles.");
            foreach (RaycastHit2D hit in hits)
            {
                Debug.Log($"[ObstacleTransparency] Detected obstacle: {hit.collider.gameObject.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            }
        }
        else
        {
            Debug.Log("[ObstacleTransparency] Raycast detected 0 obstacles.");
        }

        foreach (RaycastHit2D hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            TilemapRenderer tilemapRenderer = hit.collider.GetComponentInParent<TilemapRenderer>(); // 타일맵 렌더러는 부모에 있을 수 있습니다.
            
            if (renderer != null && !currentlyTransparentObstacles.Contains(renderer))
            {
                // 일반 오브젝트 렌더러 처리
                StartCoroutine(FadeToTransparent(renderer));
                newObstaclesToTransparent.Add(renderer);
            }
            else if (tilemapRenderer != null && !newTilemapObstaclesToTransparent.Contains(tilemapRenderer))
            {
                // 타일맵 렌더러 처리
                StartCoroutine(FadeTilemapToTransparent(tilemapRenderer));
                newTilemapObstaclesToTransparent.Add(tilemapRenderer);
            }
        }

        // 이전에 투명했지만 더 이상 감지되지 않는 장애물은 불투명하게 만듭니다.
        List<Renderer> obstaclesToRestore = new List<Renderer>();
        foreach (Renderer renderer in currentlyTransparentObstacles)
        {
            if (!newObstaclesToTransparent.Contains(renderer))
            {
                obstaclesToRestore.Add(renderer);
            }
        }

        foreach (Renderer renderer in obstaclesToRestore)
        {
            StartCoroutine(FadeToOpaque(renderer));
        }

        // 타일맵 렌더러 복원
        List<TilemapRenderer> tilemapObstaclesToRestore = new List<TilemapRenderer>();
        foreach (TilemapRenderer tilemapRenderer in currentlyTransparentTilemapObstacles)
        {
            if (!newTilemapObstaclesToTransparent.Contains(tilemapRenderer))
            {
                tilemapObstaclesToRestore.Add(tilemapRenderer);
            }
        }

        foreach (TilemapRenderer tilemapRenderer in tilemapObstaclesToRestore)
        {
            StartCoroutine(FadeTilemapToOpaque(tilemapRenderer));
        }

        currentlyTransparentObstacles = newObstaclesToTransparent;
        currentlyTransparentTilemapObstacles = newTilemapObstaclesToTransparent; // 타일맵 렌더러 목록 업데이트
    }

    System.Collections.IEnumerator FadeTilemapToTransparent(TilemapRenderer tilemapRenderer)
    {
        Debug.Log($"[ObstacleTransparency] FadeTilemapToTransparent started for {tilemapRenderer.gameObject.name}.");

        if (!originalTilemapMaterials.ContainsKey(tilemapRenderer))
        {
            originalTilemapMaterials.Add(tilemapRenderer, new Material(tilemapRenderer.material)); // 복원을 위해 원본 Material 복사본 저장
        }

        Material material = tilemapRenderer.material; // Material 인스턴스를 가져옵니다.
        Color originalColor = material.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, transparentAlpha);

        // Material의 렌더링 모드를 투명으로 설정
        SetupMaterialForFade(material);

        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            material.color = Color.Lerp(originalColor, targetColor, t);
            Debug.Log($"[ObstacleTransparency] Fading {tilemapRenderer.gameObject.name}: Current alpha = {material.color.a}");
            yield return null;
        }
        material.color = targetColor;
        Debug.Log($"[ObstacleTransparency] Finished fade for {tilemapRenderer.gameObject.name}. Final alpha = {material.color.a}.");
    }

    System.Collections.IEnumerator FadeTilemapToOpaque(TilemapRenderer tilemapRenderer)
    {
        if (!originalTilemapMaterials.ContainsKey(tilemapRenderer))
        {
            yield break; // 원래 재질이 없으면 복원할 수 없습니다.
        }

        Material material = tilemapRenderer.material;
        Material originalMaterial = originalTilemapMaterials[tilemapRenderer];

        Color transparentColor = material.color;
        Color targetColor = originalMaterial.color; // 원래 색상으로 복원

        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            material.color = Color.Lerp(transparentColor, targetColor, t);
            yield return null;
        }
        material.color = targetColor;

        // 렌더링 모드를 불투명으로 설정
        SetupMaterialForOpaque(material);

        originalTilemapMaterials.Remove(tilemapRenderer);
    }

    // 투명하게 페이드 인/아웃 코루틴
    System.Collections.IEnumerator FadeToTransparent(Renderer renderer)
    {
        if (!originalMaterials.ContainsKey(renderer))
        {
            originalMaterials.Add(renderer, renderer.material);
        }

        Material material = renderer.material;
        Color originalColor = material.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, transparentAlpha);

        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            material.color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
        }
        material.color = targetColor;
        
        // 렌더링 모드를 Fade 또는 Transparent로 설정
        SetupMaterialForFade(material);
    }

    System.Collections.IEnumerator FadeToOpaque(Renderer renderer)
    {
        if (!originalMaterials.ContainsKey(renderer))
        {
            // 원래 재질이 없으면 복원할 수 없습니다.
            yield break;
        }

        Material material = renderer.material;
        Material originalMaterial = originalMaterials[renderer];

        Color transparentColor = material.color;
        Color targetColor = originalMaterial.color; // 원래 색상으로 복원

        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            material.color = Color.Lerp(transparentColor, targetColor, t);
            yield return null;
        }
        material.color = targetColor;

        // 렌더링 모드를 Opaque로 설정
        SetupMaterialForOpaque(material);

        // 원래 재질로 되돌릴 필요가 있다면 여기에 추가 (현재는 색상만 되돌림)
        // renderer.material = originalMaterials[renderer]; 
        originalMaterials.Remove(renderer);
    }

    void SetupMaterialForFade(Material mat)
    {
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000; // Transparent 렌더 큐
    }

    void SetupMaterialForOpaque(Material mat)
    {
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.DisableKeyword("_ALPHATEST_ON"); // 또는 _ALPHATEST_ON을 활성화할 수도 있습니다.
        mat.renderQueue = -1; // 기본 렌더 큐 (또는 Opaque 렌더 큐)
    }

    // --- DEBUGGING VISUALIZATION ---
    void OnDrawGizmos()
    {
        if (player == null)
            return;

        Vector3 cameraPosition = transform.position;
        Vector3 playerPosition = player.position;
        Vector3 direction = (playerPosition - cameraPosition).normalized;
        float distance = Vector3.Distance(playerPosition, cameraPosition);

        // 레이캐스트 경로를 초록색 선으로 표시
        Gizmos.color = Color.green;
        Gizmos.DrawLine(cameraPosition, playerPosition);

        // 레이캐스트에 감지된 모든 지점을 빨간색 구로 표시
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(new Ray(cameraPosition, direction), distance, obstacleLayer);
        foreach (RaycastHit2D hit in hits)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }
}
