using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    public bool autoHitOnEnable;
    public float enableHitDamage = 300;
    public List<HealthModule> healthModulesInRange;
    private void OnTriggerEnter(Collider other)
    {
        HealthModule healthModule = other.GetComponent<HealthModule>();
        if (healthModule != null) healthModulesInRange.Add(healthModule);
    }

    private void OnTriggerExit(Collider other)
    {
        HealthModule healthModule = other.GetComponent<HealthModule>();
        if (healthModule != null) healthModulesInRange.Remove(healthModule);
    }

    private void OnEnable()
    {
        if (autoHitOnEnable) Attack(enableHitDamage);
    }

    public void Attack(float damage)
    {
        foreach (HealthModule healthModule in healthModulesInRange)
        {
            healthModule.TakeDamage(damage);
        }
    }
}
