using System.Collections;
using System.Collections.Generic;
using DunGen;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static Room activeRoom;

    public List<GameObject> initialEnemies;
    public List<Enemy> activeEnemies;

    public bool isPlaying;

    Tile tile;

    private void Awake()
    {
        tile = GetComponentInParent<Tile>();
    }

    private void Start()
    {
        foreach (var enemy in initialEnemies)
        {
            // Random Position and Rotation
            var position = transform.position + new Vector3(Random.Range(-5, 5), 2.5f, Random.Range(-5, 5));
            var rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            var enemyInstance = Instantiate(enemy, position, rotation);
            activeEnemies.Add(enemyInstance.GetComponent<Enemy>());
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
        activeRoom = this;
        DungeonDoorLink.Instance.SetTileDoors(tile, false);
        isPlaying = true;
    }

    public void Win()
    {
        DungeonDoorLink.Instance.SetTileDoors(tile, true);
        isPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Agak kurang rapi
            Invoke("Play", 2f);
        }
    }
}