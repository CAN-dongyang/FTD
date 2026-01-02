using UnityEngine;
using UnityEngine.UI;

public class MinimapUI : MonoBehaviour
{
    [Header("Minimap Elements")]
    public RawImage mapTextureImage; // Render Texture가 표시될 UI
    public Button zoomInButton;
    public Button zoomOutButton;

    void OnEnable()
    {
        zoomInButton?.onClick.AddListener(ZoomIn);
        zoomOutButton?.onClick.AddListener(ZoomOut);
    }
	void OnDestroy()
	{
		zoomInButton?.onClick.RemoveListener(ZoomIn);
		zoomOutButton?.onClick.RemoveListener(ZoomIn);
	}

    public void ZoomIn()
    {
        Debug.Log("Minimap Zoom In");
    }

    public void ZoomOut()
    {
        Debug.Log("Minimap Zoom Out");
    }

    // 외부에서 미니맵 텍스처를 설정할 수 있는 함수
    public void SetMapTexture(RenderTexture texture)
    {
        if (mapTextureImage != null)
        {
            mapTextureImage.texture = texture;
        }
    }
}
