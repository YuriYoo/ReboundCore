using UnityEngine;
using Sirenix.OdinInspector;

public class SkillManager : MonoBehaviour
{
	public static SkillManager Instance { get; private set; }

	[Header("技能配置")]
	[LabelText("当前装备的技能")]
	public SkillData equippedSkill;

	[Header("能量系统")]

	/// <summary>能量上限</summary>
	public float maxEnergy = 100f;

	/// <summary>每秒回复的能量</summary>
	public float energyRegenRate = 10f;

	[ProgressBar(0, nameof(maxEnergy))]
	public float currentEnergy = 0f;

	// 全局状态变量（被技能修改）
	[HideInInspector]
	public float globalSpeedMultiplier = 1f;

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		// 随时间自然回复能量
		if (currentEnergy < maxEnergy)
		{
			currentEnergy += energyRegenRate * Time.deltaTime;
			currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
		}

		// 监听玩家输入（按下空格键）释放技能
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CastSkill();
		}
	}

	/// <summary>
	/// 释放技能
	/// </summary>
	private void CastSkill()
	{
		if (equippedSkill == null || equippedSkill.skillEffect == null) return;

		if (currentEnergy >= equippedSkill.energyCost)
		{
			// 扣除能量
			currentEnergy -= equippedSkill.energyCost;

			// 执行技能逻辑
			equippedSkill.skillEffect.Execute(this);
		}
		else
		{
			Debug.Log($"能量不足！需要 {equippedSkill.energyCost}，当前 {Mathf.FloorToInt(currentEnergy)}");
		}
	}
}