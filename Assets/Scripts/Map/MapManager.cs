using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public List<Tilemap> collisionTilemaps;

    // --- 여기가 추가되었습니다 ---
    // 맵 데이터가 준비되었음을 알리는 '신호등' 역할을 합니다.
    public bool IsInitialized { get; private set; } = false;

    private HashSet<Vector3Int> collisionTiles = new HashSet<Vector3Int>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            CacheCollisionTiles();
        }
    }

    private void CacheCollisionTiles()
    {
        collisionTiles.Clear();
        if (collisionTilemaps == null) return;

        foreach (var tilemap in collisionTilemaps)
        {
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    collisionTiles.Add(pos);
                }
            }
        }
        Debug.Log($"[MapManager] {collisionTiles.Count}개의 장애물 타일을 캐시했습니다. 이제 준비 완료입니다.");
        
        // --- 여기가 추가되었습니다 ---
        // 모든 장애물 타일 캐시가 끝나면 '초록불'을 켭니다.
        IsInitialized = true;
    }

    public bool IsCollision(Vector3Int cellPosition)
    {
        return collisionTiles.Contains(cellPosition);
    }
}

