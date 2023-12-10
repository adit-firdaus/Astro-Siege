using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public float moveSpeed = 1;
    public float detectionRange = 10f;
    public float attackRange = 5f;
    public float attackDamage = 10f;
    public float shootInterval = 2f;
    float shottTimer = 0f;
    public GameObject explosionPrefab;
    public Animator animator;
    public GameObject BulletPrefabs;
    public Transform pos;
    public float bulletSpeed = 20;
    public AudioSource As;
    public AudioClip shootSound;

    bool playerInRange = false;
    bool canAttackPlayer = false;

    private void Update()
    {
        if (!player) return;

        

        if (shottTimer >= shootInterval)
        {
            shottTimer = 0f;
            Shoot();
        }

        playerInRange = Vector3.Distance(transform.position, player.transform.position) < detectionRange;
        canAttackPlayer = Vector3.Distance(transform.position, player.transform.position) < attackRange;

        if (playerInRange && !canAttackPlayer)
        {
            rb.velocity = transform.forward * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        if (playerInRange)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            rb.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
        if (canAttackPlayer)
        {
            shottTimer += Time.deltaTime;
        }

        Vector3 Dir = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        gameObject.transform.LookAt(Dir);
        
    }

    public void Shoot()
    {
        if (canAttackPlayer)
        {
            Rigidbody rb = Instantiate(BulletPrefabs, pos.position, transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
            animator.SetTrigger("isAttack");
            As.PlayOneShot(shootSound);
        }
    }


    public override void OnDie()
    {
        base.OnDie();

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        DestroyEnemy();
    }
}
