using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gear", menuName = "数据/配件数据")]
public class GearData : ScriptableObject
{
    [FoldoutGroup("基础信息")]
    [LabelText("配件名称")]
    public string gearName;

    [FoldoutGroup("基础信息")]
    [LabelText("部位")]
    public GearType type;

    [FoldoutGroup("基础信息")]
    [LabelText("图标"), PreviewField(50)]
    public Sprite icon;

    [FoldoutGroup("基础属性加成")]
    [LabelText("攻击力加成")]
    public int bonusAttackPower;

    [FoldoutGroup("基础属性加成")]
    [LabelText("弹射速度加成")]
    public float bonusSpeed;

    [FoldoutGroup("基础属性加成")]
    [LabelText("子弹体积加成")]
    public float bonusSize;

    [FoldoutGroup("基础属性加成")]
    [LabelText("暴击率加成 (%)")]
    [PropertyRange(0, 100)]
    public float bonusCritRate;

    [Space(20)]

    [FoldoutGroup("高级词缀")]
    [LabelText("附带词缀列表")]
    [SerializeReference] // 序列化一个包含多种不同子类的列表！
    public List<AffixEffect> affixes = new();
}