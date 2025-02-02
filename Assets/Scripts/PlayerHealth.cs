using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float playerHitPoints = 100f;
    public HealthBar healthBar;
    public float currentHealth;
    private void Start()
    {
        healthBar.setMaxHealth(playerHitPoints);
    }
    public void PlayerTakeDamge(float damage)
    {
        playerHitPoints -= damage;
        currentHealth = playerHitPoints;
        healthBar.setHealthy(currentHealth);
        if (playerHitPoints <= 0)
        {
            GetComponent<DeathHandler>().HandleDeath();
        }
    }
}
