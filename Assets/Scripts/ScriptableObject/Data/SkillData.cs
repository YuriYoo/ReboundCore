using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Skill", menuName = "数据/技能数据")]
public class SkillData : ScriptableObject
{
	[LabelText("技能名称")]
	public string skillName;

	[LabelText("能量消耗")]
	public float energyCost = 50f;

	[LabelText("技能效果配置")]
	[SerializeReference]
	public ActiveSkill skillEffect;
}