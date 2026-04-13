using UnityEngine;

public class Block : MonoBehaviour
{
    /// <summary>测试血量</summary>
    public int hp = 3;

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        hp -= damage;

        // TODO: 加入 DoTween 的受击抖动特效
        Debug.Log($"方块受到 {damage} 点伤害，剩余血量: {hp}");

        if (hp <= 0)
        {
            // TODO: 加入 VFX Graph 爆炸特效和声音
            Destroy(gameObject);
        }
    }
}
