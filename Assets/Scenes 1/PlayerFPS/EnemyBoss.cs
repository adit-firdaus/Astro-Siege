using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthModule), typeof(Rigidbody))]
public class EnemyBoss : Enemy
{
    private void Awake()
    {
        healthModule = GetComponent<HealthModule>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        healthModule.onDie.AddListener(OnDie);
    }

    public void DestroyEnemyBoss()
    {
        Destroy(gameObject);
    }
}
