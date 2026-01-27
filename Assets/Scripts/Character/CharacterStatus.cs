using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터는 3가지 형태의 시너지 태그를 가진다<br/>
/// <para>특성<br/>
/// </para>
/// 
/// <para>흥미<br/>
/// </para>
/// 
/// <para>성향<br/>
/// </para>
/// 캐릭터는 3가지 분류의 스탯을 가진다
/// <para>
/// </para>
/// </summary>
[Serializable]
public struct CharacterStatus
{
	/// <summary>
	/// 
	/// </summary>
	public Ability[] aptitudes;
	public Ability[] interests;
	public Disposition dispositions;
	public SkillAsset[] skills;

	[Range(0, 100)]
	public float Charm, Dignity, Morality;
	[Range(0, 100)]
	public float Intelligence, Mentality, Creativity;
	[Range(0, 100)]
	public float Strength, Agility, Technique;
}