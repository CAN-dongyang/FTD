using UnityEngine;
using UnityEngine.U2D;

// 모든 캐릭터의 베이스.
[CreateAssetMenu(fileName = "Character", menuName = "Asset/Character")]
public class CharacterAsset : ScriptableObject
{
	[SerializeField] private string _name;
	[SerializeField, TextArea] private string _description;

	[Header("Character")]
	public CharacterStatus _baseStatus;

	[Header("Image")]
	[SerializeField] private SpriteAtlas _spriteAtlas;

	[Header("Text")]
	[SerializeField] private TextAsset _textData;

	[Header("Synergy")]
	[SerializeField] private SynergyData[] _synergy;
}