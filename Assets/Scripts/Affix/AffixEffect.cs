using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

/// <summary>
/// 基础词缀类 (所有词缀的祖先)
/// </summary>
[Serializable, HideReferenceObjectPicker] // 隐藏冗余的类型选择 UI
public abstract class AffixEffect
{
	/// <summary>
	/// 定义一个通用的触发接口。
	/// 只要子弹撞击了方块，就会遍历调用所有词缀的这个方法。
	/// </summary>
	/// <param name="projectile"></param>
	/// <param name="targetBlock"></param>
	public abstract void ExecuteOnHit(CoreProjectile projectile, Block targetBlock);
}

/// <summary>
/// 具体实现：分裂词缀
/// </summary>
[Serializable]
public class SplitAffix : AffixEffect
{
	[LabelText("分裂数量")]
	public int splitCount = 2;

	[LabelText("触发所需撞击次数")]
	public int requiredHits = 3;

	[LabelText("分裂子弹的速度衰减倍率")]
	public float speedMultiplier = 0.8f; // 让小弹珠飞得稍微慢一点，更有层次感

	public override void ExecuteOnHit(CoreProjectile projectile, Block targetBlock)
	{
		// 如果是克隆体，就不再无限分裂了（后期可以做天赋解锁“子弹可裂变2次”）
		if (projectile.isClone) return;

		// 是否达到了触发所需的撞击次数 (利用取余 % 来实现每撞 N 次触发一次)
		if (projectile.totalHitCount % requiredHits == 0)
		{
			// 基础扩散角度
			float spreadAngle = 30f;

			for (int i = 0; i < splitCount; i++)
			{
				// 实例化克隆体
				var cloneProjectile = GameObject.Instantiate<CoreProjectile>(projectile, projectile.transform.position, Quaternion.identity);

				// 标记为克隆体，防止死循环
				cloneProjectile.isClone = true;

				// 计算新的散布方向 (利用四元数进行向量旋转)
				// 给每个子弹一个随机的偏移角度
				float randomOffset = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
				Vector2 originalDirection = projectile.CurrentVelocity.normalized;
				Vector2 newDirection = Quaternion.Euler(0, 0, randomOffset) * originalDirection;

				// 赋予新子弹速度
				float currentSpeed = projectile.CurrentVelocity.magnitude;
				cloneProjectile.CurrentVelocity = newDirection * (currentSpeed * speedMultiplier);
			}
		}
	}
}

/// <summary>
/// 具体实现：元素附着词缀
/// </summary>
[Serializable]
public class ElementAffix : AffixEffect
{
	[LabelText("元素类型")]
	public ElementType elementType;

	[LabelText("触发概率 (%)")]
	[PropertyRange(0, 100)]
	public float triggerChance = 25f;

	[LabelText("持续伤害/减速倍率")]
	public float effectValue = 5f;

	[LabelText("持续时间 (秒)")]
	public float duration = 3f;

	public override void ExecuteOnHit(CoreProjectile projectile, Block targetBlock)
	{
		if (Random.Range(0f, 100f) <= triggerChance)
		{
			// 根据不同的元素类型，触发不同效果
			switch (elementType)
			{
				case ElementType.Fire:
					// 触发燃烧！给方块挂上 DoT 状态
					targetBlock.ApplyBurn(effectValue, duration);
					break;

				case ElementType.Ice:
					// TODO: 留给后续扩展，比如降低方块的某个防御力系数，或者把方块变成蓝色易碎冰块
					Debug.Log("触发冰冻！");
					break;

				case ElementType.Light:
					// TODO: 留给后续扩展，比如发射一条射线寻找最近的另一个方块造成连锁伤害
					Debug.Log("触发闪电连锁！");
					break;

				case ElementType.Dark:
					Debug.Log("触发黑暗效果！");
					break;
			}
		}
	}
}