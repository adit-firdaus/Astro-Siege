using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarEnemy : Enemy
{
    public float moveSpeed = 1;
    public float detectionRange = 25f;
    public float attackRange = 25f;
    public float attackDamage = 10f;
    public GameObject explosionPrefab;
    public Animator anim;
    bool playerInRange = false;
    bool canAttackPlayer = false;
    public GameObject Projectile;
    public Transform ProjPos;
    private float attkCooldown = 1f;
    public AudioSource AS;
    public AudioClip screek;
    private bool isAttacking;
    private SonarWave Sw;

    private void Update()
    {
        if (!player) return;

        if (isDead)
        {
            return;
        }

        playerInRange = Vector3.Distance(transform.position, player.transform.position) < detectionRange;
        canAttackPlayer = Vector3.Distance(transform.position, player.transform.position) < attackRange;

        if (playerInRange && !canAttackPlayer)
        {
            rb.velocity = transform.forward * moveSpeed;
            anim.SetBool("isWalking", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("isWalking", false);
        }
        if (isAttacking == true)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            rb.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
        if (playerInRange)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            rb.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
            if (!isAttacking)
            {
                attkCooldown -= Time.deltaTime;
            }

        }
        if (canAttackPlayer && attkCooldown <= 0)
        {
            isAttacking = true;
            anim.SetTrigger("isAttack");
            attkCooldown = 8f;
            AS.PlayOneShot(screek);
            AttackPlayer();

        }



    }
    public void AttackPlayer()
    {
        GameObject sWave;
        sWave = Instantiate(Projectile, ProjPos.transform.position, Quaternion.identity);
        Sw = sWave.GetComponent<SonarWave>();
        sWave.transform.parent = ProjPos.transform;
    }

    public void endAttack()
    {
        isAttacking = false;
        if (!Sw)
            return;
        Sw.release();
    }
    public override void OnDie()
    {
        base.OnDie();

        if (!isDead)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            anim.SetBool("isDead", true);
            isDead = true;
        }
    }
}
