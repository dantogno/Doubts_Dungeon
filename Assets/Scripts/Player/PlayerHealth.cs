using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public int healthPickups;

    //Made this public so trap can check if player can take damage, probably better way to do this
    public bool canTakeDamage = true;

    public float damageCooldownDuration = 1f;

    [SerializeField]
    public CameraEffect CE;
    

    public void Update()
    {
        CheckForHealthPickup();
    }

    

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void GainHealth(int increase)
    {
        health += increase;
    }

    public void PickupHealth()
    {
        healthPickups++;
    }

    bool usable;
    public void UseHealth()
    {
        usable = false;
        if (health < maxHealth)
        {
            healthPickups--;
            usable = true;
        }

        if (usable) { GainHealth(1); }
    }

    public void PlayerHasBeenHit()
    {
        canTakeDamage = false;
        StartCoroutine(DamageCooldown());
        // Reduce player's health when colliding with an enemy
        ManageDamage(1);
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldownDuration);
        canTakeDamage = true; // Allow taking damage again after cooldown
    }

    void ManageDamage(int damage)
    {
        if (GetHealth() > 0)
        {
            TakeDamage(damage);

            MusicManager.instance.SetLowPassCutoffBasedOnHealth((float)GetHealth() / (float)GetMaxHealth());
            canTakeDamage = true;
            CE.ShakeCamera();
            // Check for player death
            if (GetHealth() == 0)
            {
                CE.StopCameraShake();
                //OnGameOver();
            }

        }
    }

    internal void CheckForHealthPickup()
    {
        if (Input.GetButtonDown("UseHealth"))
        {
            Debug.Log("Player hit: UseHealth | E key");
            UseHealth();
        }
    }


}
