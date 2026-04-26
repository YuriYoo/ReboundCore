using UnityEngine;

public class CoreProjectile : MonoBehaviour
{
    [Header("基础属性")]

    /// <summary>飞行速度</summary>
    public float speed = 15f;

    /// <summary>子弹的物理碰撞半径（需与你的贴图大小匹配）</summary>
    public float radius = 0.25f;

    /// <summary>基础伤害</summary>
    public float attackPower = 1;

    [Header("碰撞设置")]
    public LayerMask collisionMask;

    // 测试用

    [Header("装备测试")]
    public GearData equippedGear;

    // 运行时变量

    // 状态记录

    /// <summary>总共撞击了多少次方块</summary>
    [HideInInspector] public int totalHitCount = 0;

    /// <summary>是否是分裂出来的克隆体</summary>
    [HideInInspector] public bool isClone = false;

    private SpriteRenderer sr;

    /// <summary>当前的速度向量</summary>
    public Vector2 CurrentVelocity { get; set; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // 如果是克隆体，不要重新计算初始方向，直接用词缀赋予的方向
        if (!isClone)
        {
            // 测试用：启动时给一个随机的右上角发射方向
            Vector2 initialDirection = new Vector2(Random.Range(0.5f, 1f), 1f).normalized;

            // 如果有装备，把装备的属性加成算上
            float finalSpeed = speed;
            if (equippedGear != null)
            {
                finalSpeed += equippedGear.bonusSpeed;
                attackPower += equippedGear.bonusAttackPower;
                // radius 等属性也可以在此处叠加
            }

            CurrentVelocity = initialDirection * finalSpeed;
        }
        else
        {
            sr.color = Color.green;
        }
    }

    void Update()
    {
        MoveAndBounce();
    }

    private void MoveAndBounce()
    {
        // 获取包含技能加成的最终真实速度向量
        Vector2 actualVelocity = CurrentVelocity * SkillManager.Instance.globalSpeedMultiplier;

        // 根据真实速度计算这帧的移动距离和方向
        //float distanceThisFrame = speed * Time.deltaTime;
        float distanceThisFrame = actualVelocity.magnitude * Time.deltaTime;

        Vector2 direction = actualVelocity.normalized;

        // 核心：发射圆形射线，预测下一帧的移动路径是否会撞到东西
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, direction, distanceThisFrame, collisionMask);

        if (hit.collider != null)
        {
            // 1. 发生碰撞！先把子弹瞬间移动到碰撞点（使用 centroid 确保圆心停留在贴边位置）并沿着法线往外稍微推一点点 (0.01f)，防止下一帧卡墙
            transform.position = hit.centroid + hit.normal * 0.01f;

            // 2. 核心数学：利用法线（hit.normal）计算反射向量
            CurrentVelocity = Vector2.Reflect(CurrentVelocity, hit.normal);

            // 3. 业务逻辑：判断撞到了什么
            if (hit.collider.TryGetComponent<Block>(out var targetBlock))
            {
                targetBlock.TakeDamage(attackPower);

                // 记录撞击次数
                totalHitCount++;

                // 词缀系统的核心枢纽
                // 击中方块后，遍历当前装备的所有词缀，并执行它们的逻辑
                if (equippedGear != null && equippedGear.affixes != null)
                {
                    foreach (var affix in equippedGear.affixes)
                    {
                        // 把当前子弹 (this) 和被击中的方块传给词缀，让词缀自己决定要干嘛
                        affix.ExecuteOnHit(this, targetBlock);
                    }
                }
            }
        }
        else
        {
            // 如果没撞到任何东西，正常向前移动
            transform.Translate(actualVelocity * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// 在编辑器里画出子弹的碰撞半径
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}