using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherEnemy : Enemy
{
    public float moveSpeed = 1;
    public float detectionRange = 10f;
    public float attackRange = 5f;
    public float attackDamage = 10f;
    public float dashSpeed = 10f;
    public float dashWaitTime = 1f;
    public GameObject impactEffect;

    bool playerInRange = false;
    bool canAttackPlayer = false;

    bool prepareToDash = false;
    bool dashed = false;

    float dashTimer = 0f;
    public Animator anim;
    public AudioSource As;
    public AudioClip Taunt;

    private void Start()
    {
        As = GetComponent<AudioSource>();
    }

    private void Update()
    {

        if (!player) return;

        if (dashed)
        {
            return;
        }

        if (prepareToDash)
        {
            dashTimer += Time.deltaTime;

            anim.SetTrigger("taunt");

            if (dashTimer >= dashWaitTime)
            {
                StartCoroutine("Dash");
                dashed = true;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
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
            prepareToDash = true;
        }

        if (playerInRange)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            rb.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    IEnumerator Dash()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.4f);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
    }
    public void TauntSound()
    {
        As.PlayOneShot(Taunt);
    }

    public override void OnDie()
    {
        base.OnDie();

        DestroyEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (dashed)
        {
            var player = collision.transform.GetComponentInParent<Player>();
            if (player) player.healthModule.TakeDamage(attackDamage);
            HitDestroy();

        }
    }

    public void HitDestroy()
    {
        if (impactEffect) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
