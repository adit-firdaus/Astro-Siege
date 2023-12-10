using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicEnemy : Enemy
{
    public int level = 1;
    public float moveSpeed = 1f;
    public float attackRange = 1f;
    public float health = 200;
    public float damage = 50;

    public MeleeDamage meleeArea;
    public Vector3 moveDirection;
    public Animator animator;
    public bool isAttacking = false;

    public bool canAttackPlayer = false;

    private void Start()
    {
        health = 200 * Mathf.Pow(level, 2);
        damage = 50 * level;
    }

    private void OnDrawGizmos()
    {
        // Draw semuanya
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw hanya ketika player ada
        if (player)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (!player)
        {
            return;
        }

        if (player)
        {
            canAttackPlayer = Vector3.Distance(transform.position, player.transform.position) < attackRange;
            animator.SetBool("CanAttack", canAttackPlayer);

        }

        if (canAttackPlayer == false && isAttacking == false)
        {
            Chase();
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
        meleeArea.Attack(damage);
        // Debug.Log("ngen");
    }

    public void endAttack()
    {
        isAttacking = false;
        // Debug.Log("tot");
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

    public void Death()
    {
        animator.SetTrigger("Dead");
    }
}
