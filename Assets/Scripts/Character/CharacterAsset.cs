using UnityEngine;
using UnityEngine.U2D;

// 모든 캐릭터의 베이스.
[CreateAssetMenu(fileName = "New Character", menuName = "Asset/Character")]
public class CharacterAsset : BaseAssetSO
{
	[Header("Character")]
	[SerializeField] private CharacterStatus _baseStatus;

	[Header("Image")]
	[SerializeField] private SpriteAtlas _spriteAtlas;
	[SerializeField] private TextAsset _textData;
}