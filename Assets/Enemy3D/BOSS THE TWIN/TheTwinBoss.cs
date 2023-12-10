using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTwinBoss : EnemyBoss
{
    public List<GameObject> MiniBosses;
    public GameObject BloodEffect;
    public float speed = 10f;
    public float forceChangeDirectionTime = 2;

    float time;

    public void Update()
    {
        time += Time.deltaTime;

        if (time >= forceChangeDirectionTime)
        {
            time = 0;
            Changedirection();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Changedirection();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Changedirection();
        }
    }

    public override void OnDie()
    {
        base.OnDie();
        SpawnMiniBoss();
        Instantiate(BloodEffect, transform.position, Quaternion.identity);
        Debug.Log("Boss Die");
        Destroy(gameObject);
    }

    public void SpawnMiniBoss()
    {
        for (int i = 0; i < MiniBosses.Count; i++)
        {
            var enemy = Instantiate(MiniBosses[i], transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.room = room;
            room.activeEnemies.Add(enemy);
        }
    }

    public void Changedirection()
    {
        float dir = Random.Range(-60, 60);
        gameObject.GetComponent<Transform>().Rotate(0, 180 + dir, 0);
    }
}
