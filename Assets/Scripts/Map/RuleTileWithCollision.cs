using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 충돌 여부를 설정할 수 있는 커스텀 타일 클래스.
/// </summary>
[CreateAssetMenu(fileName = "New Rule Tile With Collision", menuName = "Tiles/Rule Tile With Collision")]
public class RuleTileWithCollision : RuleTile
{
    [Header("Collision Settings")]
    [Tooltip("이 타일이 충돌을 발생하는지 여부 (true이면 이동 불가)")]
    public bool isWall = false;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        // 이 타일의 충돌 정보를 설정합니다.
        // RuleTile은 기본적으로 ColliderType을 Sprite로 설정하므로,
        // isWall 값에 따라 ColliderType을 변경할 수 있습니다.
        if (isWall)
        {
            tileData.colliderType = Tile.ColliderType.Grid;
        }
        else
        {
            tileData.colliderType = Tile.ColliderType.None;
        }
    }
}
