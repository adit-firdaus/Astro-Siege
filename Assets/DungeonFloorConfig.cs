using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonFloorConfig", menuName = "DungeonFloorConfig")]
public class DungeonFloorConfig : ScriptableObject
{
    public int totalRoom = 5;
    public int totalTreasureRoom = 1;
    public GameObject playerPrefab;
    public List<DungeonEnemyConfig> enemyConfigs = new List<DungeonEnemyConfig>();
    public EnemyBoss bossPrefab;

    public List<List<GameObject>> GetEnemyPrefabs()
    {
        List<List<GameObject>> allEnemyLevels = new();

        for (int i = 0; i < totalRoom; i++)
        {
            var prefabs = new List<GameObject>();

            foreach (var enemyConfig in enemyConfigs)
            {
                float average = enemyConfig.total / enemyConfig.maxCount;

                prefabs.AddRange(Enumerable.Repeat(enemyConfig.enemyPrefab, (int)average));
            }

            allEnemyLevels.Add(prefabs);
        }

        return allEnemyLevels;
    }
}

[System.Serializable]
public class DungeonEnemyConfig
{
    public GameObject enemyPrefab;
    public int total = 1;
    public int maxCount = 1;
    public int minLevel = 1;
    public int maxLevel = 1;

    public int GetLevel()
    {
        return Random.Range(minLevel, maxLevel + 1);
    }
}