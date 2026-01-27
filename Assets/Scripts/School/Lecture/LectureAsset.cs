using UnityEngine;

[CreateAssetMenu(fileName = "Lecture", menuName = "Asset/Lecture")]
public class LectureAsset : ScriptableObject
{
	[SerializeField] private string _name;
	[SerializeField] private string _description;
	[SerializeField] private Sprite _icon;
}