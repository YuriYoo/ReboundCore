using System;
using UnityEngine;
using Sirenix.OdinInspector;

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

    public override void ExecuteOnHit(CoreProjectile projectile, Block targetBlock)
    {
        // 具体的物理生成逻辑我们在下一阶段写，这里先留好接口
        Debug.Log($"【分裂词缀】触发！准备生成 {splitCount} 发子弹！");
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

    public override void ExecuteOnHit(CoreProjectile projectile, Block targetBlock)
    {
        Debug.Log($"【元素词缀】判定中... 类型: {elementType}");
    }
}