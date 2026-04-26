using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("当前关卡")]
    [LabelText("关卡数据文件")]
    public LevelData currentLevel;

    [Header("方块预制体配置")]
    public Block normalBlockPrefab;
    // TODO: 后续我们再做炸药桶和宝箱怪的逻辑，先留好坑位
    public Block explosiveBlockPrefab;
    public Block mimicBlockPrefab;

    [Header("生成区域 (对角线顶点)")]
    public Transform spawnAreaTopLeft;
    public Transform spawnAreaBottomRight;

    [ShowInInspector, ReadOnly, LabelText("当前波次")]
    private int currentWaveIndex = 0;

    // 记录场上存活的方块
    private List<Block> activeBlocks = new();

    void Awake()
    {
        Instance = this;
    }

    [Button("手动生成下一波 (测试用)", ButtonSizes.Large)]
    [GUIColor(0.4f, 0.8f, 1f)]
    public void SpawnNextWave()
    {
        if (currentLevel == null)
        {
            Debug.LogWarning("未配置关卡数据！");
            return;
        }

        if (currentWaveIndex >= currentLevel.waves.Count)
        {
            Debug.Log("🎉 关卡已通关！准备结算...");
            return;
        }

        StartWave(currentWaveIndex);
    }

    private void StartWave(int index)
    {
        Debug.Log($"--- 开始第 {index + 1} 波 ---");
        WaveInfo wave = currentLevel.waves[index];

        // 生成各类型方块
        SpawnBlocks(normalBlockPrefab, wave.normalBlockCount, wave.hpMultiplier);
        // SpawnBlocks(explosiveBlockPrefab, wave.explosiveCount, wave.hpMultiplier);
        // SpawnBlocks(mimicBlockPrefab, wave.mimicCount, wave.hpMultiplier);

        currentWaveIndex++;
    }

    /// <summary>
    /// 生成方块
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="count"></param>
    /// <param name="hpMultiplier"></param>
    private void SpawnBlocks(Block prefab, int count, float hpMultiplier)
    {
        if (prefab == null || count <= 0) return;

        for (int i = 0; i < count; i++)
        {
            // 在划定的区域内随机取一个坐标
            float randomX = Random.Range(spawnAreaTopLeft.position.x, spawnAreaBottomRight.position.x);
            float randomY = Random.Range(spawnAreaBottomRight.position.y, spawnAreaTopLeft.position.y);
            Vector2 spawnPos = new(randomX, randomY);

            // 实例化方块
            Block block = Instantiate(prefab, spawnPos, Quaternion.identity);

            if (block != null)
            {
                // 叠加上这一波的血量倍率
                block.hp *= hpMultiplier;
                activeBlocks.Add(block);
            }
        }
    }
}