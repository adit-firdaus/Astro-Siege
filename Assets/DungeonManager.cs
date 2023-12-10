using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DunGen;
using DunGen.Graph;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonManager : MonoBehaviour, IDungeonCompleteReceiver
{
    static DungeonManager instance;
    public static DungeonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DungeonManager>();
            }
            return instance;
        }
    }
    public RuntimeDungeon runtimeDungeon;
    public DungeonFloorConfig dungeonFloorConfig;

    public List<Room> rooms = new List<Room>();
    public List<List<GameObject>> dungeonEnemies = new List<List<GameObject>>();

    public DungeonFlow dungeonFlow => runtimeDungeon.Generator.DungeonFlow;

    private void Awake()
    {

    }

    private void Start()
    {
        var totalRoom = dungeonFloorConfig.totalRoom;

        dungeonFlow.Length = new IntRange(totalRoom, totalRoom);

        runtimeDungeon.Generate();
    }

    public void OnDungeonComplete(Dungeon dungeon)
    {
        var tiles = dungeon.AllTiles;

        rooms = tiles.Select(x => x.GetComponentInChildren<Room>()).ToList();

        foreach (var room in rooms)
        {
            var enemyConfigs = dungeonFloorConfig.GetRooomEnemyConfig(room.tile.Placement.PathDepth);

            room.enemyConfigs = enemyConfigs;
        }

        DungeonDoorLink.Instance.SetOpen(rooms[0].tile, true);
    }
}