using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthModule : MonoBehaviour
{
    public float maxHealth = 100;
    public float health = 100;
    public UnityEvent onDie;
    public AudioSource As;
    public AudioClip ImpactEffect;
    public Slider HealthBar;
    public GameObject DeathScreen;
    public GameObject deathHealth;

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
        if (deathHealth)
        {
            deathHealth.SetActive(true);
            Invoke("Matiin", 2f);
        }
        if (health <= 0)
        {
            onDie.Invoke();
            if (DeathScreen)
            {
                DeathScreen.SetActive(true);
                GameObject.FindObjectOfType<PlayerController>().isDead = true;
            }
        }
    }
    public void Matiin()
    {
        deathHealth.SetActive(false);
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
    private void Update()
    {
        if (HealthBar)
        {
            HealthBar.value = health;
            HealthBar.maxValue = maxHealth;
        }
    }

}
