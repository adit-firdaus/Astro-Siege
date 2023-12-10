using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthModule), typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public static Player player => Player.Instance;
    protected HealthModule healthModule;
    protected Rigidbody rb;

    public bool isDead;

    private void OnDrawGizmos()
    {
        if (player)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    private void Awake()
    {
        healthModule = GetComponent<HealthModule>();
        rb = GetComponent<Rigidbody>();
    }

    public virtual void OnDie()
    {

    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
