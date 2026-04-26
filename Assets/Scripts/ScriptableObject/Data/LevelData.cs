using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

/// <summary>
/// 单波次的数据结构
/// </summary>
[Serializable]
public class WaveInfo
{
    [LabelText("普通石块数量")]
    public int normalBlockCount = 5;

    [LabelText("炸药桶数量")]
    public int explosiveCount = 0;

    [LabelText("宝箱怪数量")]
    public int mimicCount = 0;

    [LabelText("怪物血量倍率")]
    [MinValue(0.1f)]
    public float hpMultiplier = 1.0f;
}

[CreateAssetMenu(fileName = "New Level", menuName = "数据/关卡数据")]
public class LevelData : ScriptableObject
{
    [LabelText("关卡名称")]
    public string levelName = "";

    [TableList(ShowIndexLabels = true, AlwaysExpanded = true)]
    [LabelText("波次配置表")]
    public List<WaveInfo> waves = new();
}