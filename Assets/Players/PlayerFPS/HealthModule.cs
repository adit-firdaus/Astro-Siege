using UnityEngine;
using UnityEngine.Events;

public class HealthModule : MonoBehaviour
{
    public float maxHealth = 100;
    public float health = 100;
    public UnityEvent onDie;
    public AudioSource As;
    public AudioClip ImpactEffect;

    private void Awake()
    {
        health = maxHealth;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + Vector3.up * 1.5f, new Vector3(health / maxHealth, 0.2f, 0.2f));
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            onDie.Invoke();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth);
    }

    public void playImpactSound()
    {
        if (As && ImpactEffect) As.PlayOneShot(ImpactEffect);
    }
}
