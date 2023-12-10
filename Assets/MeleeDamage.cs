using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
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

    public void Attack(float damage)
    {
        foreach (HealthModule healthModule in healthModulesInRange)
        {
            healthModule.TakeDamage(damage);
        }
    }
}
