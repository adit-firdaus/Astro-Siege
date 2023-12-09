using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public GameObject playerPrefab;

    public void Start()
    {
        Instantiate(playerPrefab, transform.position + Vector3.up, transform.rotation);

        DungeonDoorLink.Instance.SetTileDoors(DungeonDoorLink.Instance.tiles[0], true);
    }
}
