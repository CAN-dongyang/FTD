using UnityEngine;
using UnityEngine.U2D;

// 모든 캐릭터의 베이스. Student는 이 Character로 간주
[CreateAssetMenu(fileName = "New Character", menuName = "Asset/Character")]
public class CharacterAsset : DataAsset
{
	[Header("Character")]
	[SerializeField] private SpriteAtlas _spriteAtlas;
	[SerializeField] private TextAsset _textData;

	[Header("Family")]
	[Tooltip("가문이 가질 수 있는 적성 리스트")]
	[SerializeField] private Ability[] _aptitudes;
}