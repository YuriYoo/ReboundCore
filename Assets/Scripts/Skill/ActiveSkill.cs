using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 技能逻辑基类
/// </summary>
[Serializable, HideReferenceObjectPicker]
public abstract class ActiveSkill
{
	// 技能执行的接口，我们需要传入一个 MonoBehaviour (这里传 SkillManager) 
	// 主要是因为技能往往有持续时间，需要借用 MonoBehaviour 来开启协程 (Coroutine)
	public abstract void Execute(SkillManager manager);
}

// 2. 具体实现：超频技能
[Serializable]
public class OverdriveSkill : ActiveSkill
{
	[LabelText("速度倍率")]
	public float speedMultiplier = 2f;

	[LabelText("持续时间(秒)")]
	public float duration = 5f;

	public override void Execute(SkillManager manager)
	{
		Debug.Log("【超频】技能启动！");
		manager.StartCoroutine(OverdriveRoutine(manager));
	}

	private IEnumerator OverdriveRoutine(SkillManager manager)
	{
		// 开启超频倍率
		manager.globalSpeedMultiplier = speedMultiplier;

		// 等待持续时间结束
		yield return new WaitForSeconds(duration);

		// 恢复正常倍率
		manager.globalSpeedMultiplier = 1f;
		Debug.Log("【超频】技能结束。");
	}
}