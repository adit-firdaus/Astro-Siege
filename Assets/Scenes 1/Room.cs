using System.Collections;
using System.Collections.Generic;
using DunGen;
using MyBox;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static Room activeRoom;

    public List<DungeonEnemyConfig> enemyConfigs = new List<DungeonEnemyConfig>();
    public List<Enemy> activeEnemies;

    public bool isBossRoom;
    public bool battleArena = true;
    public bool isPlaying;
    public bool isCleared;
    [ReadOnly] public int roomDepth;
    [HideInInspector] public Tile tile;
    BoxCollider tileTrigger;

    public bool hasBoss => DungeonManager.Instance.dungeonFloorConfig.bossPrefab != null;

    private void Awake()
    {
        tile = GetComponentInParent<Tile>();
    }

    private void Start()
    {
        tileTrigger = GetComponent<BoxCollider>();
        roomDepth = tile.Placement.PathDepth;

        tileTrigger.size = tileTrigger.size - new Vector3(3, 0, 3);

        if (!battleArena)
        {
            DungeonDoorLink.Instance.SetOpen(tile, true);
        }
    }

    public void SpawnEnemies()
    {
        if (isBossRoom)
        {
            if (hasBoss)
            {
                var prefab = DungeonManager.Instance.dungeonFloorConfig.bossPrefab;
                var boss = Instantiate(prefab, transform.position, Quaternion.identity);
                boss.transform.parent = transform;
                boss.GetComponent<Enemy>().room = this;
                activeEnemies.Add(boss.GetComponent<Enemy>());
            }
        }
        else
        {
            foreach (var enemyConfig in enemyConfigs)
            {
                var amount = enemyConfig.GetRandomAmount();
                for (int i = 0; i < amount; i++)
                {
                    var position = transform.position + new Vector3(Random.Range(-tileTrigger.size.x / 2, tileTrigger.size.x / 2), 0, Random.Range(-tileTrigger.size.z / 2, tileTrigger.size.z / 2));
                    var enemy = Instantiate(enemyConfig.enemyPrefab, position, Quaternion.identity);
                    enemy.transform.parent = transform;
                    enemy.GetComponent<Enemy>().room = this;
                    activeEnemies.Add(enemy.GetComponent<Enemy>());
                }
            }
        }
    }

    private void Update()
    {
        activeEnemies.RemoveAll(x => x == null);

        if (isPlaying && activeEnemies.Count == 0)
        {
            Win();
        }
    }

    public void Play()
    {
        if (isPlaying || isCleared) return;

        SpawnEnemies();
        activeRoom = this;
        DungeonDoorLink.Instance.SetOpen(tile, false);
        isPlaying = true;
        isCleared = false;
    }

    public void Win()
    {
        DungeonDoorLink.Instance.SetOpen(tile, true);
        isPlaying = false;
        isCleared = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!battleArena || (isBossRoom && !hasBoss)) return;

        if (other.CompareTag("Player"))
        {
            Play();
        }
    }
}