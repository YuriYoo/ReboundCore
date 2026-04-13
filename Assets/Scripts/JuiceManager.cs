using UnityEngine;
using DG.Tweening;
using System.Collections;

/// <summary>
/// 视觉表现力管理器
/// </summary>
public class JuiceManager : MonoBehaviour
{
	public static JuiceManager Instance { get; private set; }

	[Header("相机引用")]
	public Camera mainCamera;

	void Awake()
	{
		Instance = this;
		if (mainCamera == null) mainCamera = Camera.main;
	}

	/// <summary>
	/// 触发屏幕震动
	/// </summary>
	/// <param name="duration">震动时长</param>
	/// <param name="strength">震动幅度</param>
	public void ShakeCamera(float duration = 0.15f, float strength = 0.3f)
	{
		// 先停止之前的震动，防止连续撞击导致相机飞出屏幕
		mainCamera.DOComplete();
		// 让相机位置产生震动
		mainCamera.DOShakePosition(duration, strength, vibrato: 10, randomness: 90);
	}

	/// <summary>
	/// 触发顿帧 (Hit Stop) - 模拟强烈撞击时的瞬间停顿感
	/// </summary>
	/// <param name="duration">停顿的真实时间</param>
	public void TriggerHitStop(float duration = 0.02f)
	{
		StartCoroutine(HitStopRoutine(duration));
	}

	private IEnumerator HitStopRoutine(float duration)
	{
		// 将时间流逝速度降到极低（不能是0，否则容易卡死某些逻辑）
		Time.timeScale = 0.05f;

		// 注意：因为时间变慢了，所以等待必须使用 WaitForSecondsRealtime (真实世界时间)
		yield return new WaitForSecondsRealtime(duration);

		// 恢复正常时间
		Time.timeScale = 1f;
	}
}