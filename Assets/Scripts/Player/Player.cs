using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Player: MonoBehaviour
{
    public int health;
    public int maxHealth;

    public int healthPickups;

    //Made this public so trap can check if player can take damage, probably better way to do this
    public bool canTakeDamage = true;

    public float damageCooldownDuration = 1f;

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

        if(usable) { GainHealth(1); }
    }
}
