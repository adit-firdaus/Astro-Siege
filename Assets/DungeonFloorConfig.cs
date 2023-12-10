using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DunGen;
using MyBox;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonFloorConfig", menuName = "DungeonFloorConfig")]
public class DungeonFloorConfig : ScriptableObject
{
    public int totalRoom = 5;
    public int totalTreasureRoom = 1;
    public GameObject playerPrefab;
    public List<DungeonEnemyConfig> enemyConfigs = new List<DungeonEnemyConfig>();
    public EnemyBoss bossPrefab;

    public List<DungeonEnemyConfig> GetRooomEnemyConfig(int depth)
    {
        return enemyConfigs.Where(x => x.InDepth(depth)).ToList();
    }
}

[System.Serializable]
public class DungeonEnemyConfig
{
    public GameObject enemyPrefab;
    public IntRange amountRange = new IntRange(1, 1);
    [Range(0, 1)] public float amountProbability = 1;
    public IntRange depthRange = new IntRange(1, 1);
    public IntRange levelRange = new IntRange(1, 1);

    public bool InDepth(int depth)
    {
        return depthRange.Min <= depth && depthRange.Max >= depth;
    }

    public int GetRandomAmount()
    {
        return Random.Range(amountRange.Min, amountRange.Max + 1) * (Random.value < amountProbability ? 1 : 0);
    }
}