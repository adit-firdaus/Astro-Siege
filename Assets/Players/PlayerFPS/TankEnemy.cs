using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    public int level = 1;
    public float moveSpeed = 1f;
    public float attackRange = 1f;
    public float health = 200;
    public float damage = 50;

    public Vector3 moveDirection;
    public Animator animator;
    public bool isAttacking = false;
    public AudioSource As;
    public AudioClip RoarSound;
    public bool canAttackPlayer = false;

    private void Start()
    {
        InitStats();
    }

    public void InitStats()
    {
        health = 200 * Mathf.Pow(level, 2);
        damage = 50 * level;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (player)
        {
           if( canAttackPlayer = Vector3.Distance(transform.position, player.transform.position) < attackRange)
            {
                if (!isAttacking)
                {
                    Attack();
                }
                
            }
            // animator.SetBool("CanAttack", canAttackPlayer);
        }
        bool isOnce = false;
        if (canAttackPlayer == false && isAttacking == false)
        {
            Chase();
            if (isOnce == false)
            {
                As.PlayOneShot(RoarSound);
            }
        }

    }

    public void Patrol()
    {
        var randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

        moveDirection = Vector3.Lerp(moveDirection, randomDirection, Time.deltaTime).normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    public void Chase()
    {
        moveDirection = (player.transform.position - transform.position).normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    public void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    public void endAttack()
    {
        isAttacking = false;
    }

    public void Death()
    {
        animator.SetTrigger("Dead");
    }

    public override void OnDie()
    {
        base.OnDie();
        if (!isDead)
        {
            Death();

            isDead = true;
        }
    }
}

