using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DunGen;
using DunGen.Demo;
using UnityEngine;

public class DungeonDoorLink : MonoBehaviour, IDungeonCompleteReceiver
{
    public static DungeonDoorLink Instance;

    public List<DoorwayConnection> connections;
    public List<Tile> tiles = new List<Tile>();
    public GameObject doorPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void OnDungeonComplete(Dungeon dungeon)
    {
        tiles = dungeon.AllTiles.ToList();
    }

    public void SetTileDoors(Tile tile, bool state)
    {
        foreach (var doorway in tile.UsedDoorways)
        {
            if (doorway.HasDoorPrefabInstance)
            {
                doorPrefab = doorway.UsedDoorPrefabInstance;

                var autodoor = doorPrefab.GetComponent<AutoDoor>();

                if (autodoor != null) autodoor.SetState(state);
            }
        }
    }
}
