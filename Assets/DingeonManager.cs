using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DunGen;
using DunGen.Graph;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;

public class DingeonManager : MonoBehaviour, IDungeonCompleteReceiver
{
    public RuntimeDungeon runtimeDungeon;
    public DungeonFloorConfig dungeonFloorConfig;

    public List<Room> rooms = new List<Room>();
    public List<List<GameObject>> dungeonEnemies = new List<List<GameObject>>();

    public DungeonFlow dungeonFlow => runtimeDungeon.Generator.DungeonFlow;

    private void Start()
    {
        Init();
    }

    [ButtonMethod]
    public void Init()
    {
        var totalRoom = dungeonFloorConfig.totalRoom;

        dungeonFlow.Length = new IntRange(totalRoom, totalRoom);

        dungeonEnemies = dungeonFloorConfig.GetEnemyPrefabs();

        runtimeDungeon.Generate();
    }

    public void OnDungeonComplete(Dungeon dungeon)
    {
        var tiles = dungeon.AllTiles;

        rooms = tiles.Select(x => x.GetComponentInChildren<Room>()).ToList();

        for (int i = 0; i < rooms.Count; i++)
        {
            var room = rooms[i];
            var enemies = dungeonEnemies[i];

            room.initialEnemies = enemies;
        }
    }
}