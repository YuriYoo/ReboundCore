using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
	private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);

	/// <summary>测试血量</summary>
	public float hp = 3;

	/// <summary>是否在燃烧</summary>
	private bool isBurning = false;
	private SpriteRenderer sr;
	private Color originalColor;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		originalColor = sr.color;
	}

	/// <summary>
	/// 受到伤害
	/// </summary>
	/// <param name="damage"></param>
	public void TakeDamage(float damage)
	{
		hp -= damage;

		// 简易受击反馈：稍微变白/变红闪一下
		if (sr != null && !isBurning)
		{
			sr.color = Color.red;
			Invoke(nameof(ResetColor), 0.1f);
		}

		if (hp <= 0)
		{
			// 增强视觉效果
			if (JuiceManager.Instance != null)
			{
				// 触发中等程度的震动
				JuiceManager.Instance.ShakeCamera(0.2f, 0.2f);

				// 触发 0.03 秒的顿帧，让玩家看清方块碎裂的瞬间
				JuiceManager.Instance.TriggerHitStop(0.1f);
			}

			Destroy(gameObject);
		}
	}

	private void ResetColor()
	{
		if (sr != null && !isBurning) sr.color = originalColor;
	}

	/// <summary>
	/// 应用燃烧状态
	/// </summary>
	/// <param name="damagePerSecond"></param>
	/// <param name="duration"></param>
	public void ApplyBurn(float damagePerSecond, float duration)
	{
		// 为了防止多次撞击导致无限叠加燃烧掉血极快，我们简单做个排他处理（或者你也可以改成叠加层数）
		if (!isBurning)
		{
			StartCoroutine(BurnRoutine(damagePerSecond, duration));
		}
	}

	private IEnumerator BurnRoutine(float dps, float duration)
	{
		isBurning = true;
		float elapsed = 0f;

		// 变成橙色，表示正在被烧
		if (sr != null) sr.color = new Color(1f, 0.5f, 0f);

		while (elapsed < duration)
		{
			// 每 0.5 秒跳一次伤害，这样更有“灼烧”的节奏感
			TakeDamage(dps * 0.5f);
			yield return _waitForSeconds0_5;
			elapsed += 0.5f;
		}

		isBurning = false;
		ResetColor();
	}
}
