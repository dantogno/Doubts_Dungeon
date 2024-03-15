using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Player: MonoBehaviour
{
    //Inventory
    public List<Item> Inventory = new List<Item>();

    //Currancy
    [SerializeField]
    public int Currancy;

    //Health
    //public int health;
    //public int maxHealth;

    public int healthPickups;

    public bool canTakeDamage = true;

    public float damageCooldownDuration = .2f;

    //S T R E S S
    [SerializeField] public float stress;
    [SerializeField] public int maxStress;

    [SerializeField] int reduceStressAmount;
    float timeSinceLastHit = 0f;
    [SerializeField] float stressReductionRate = .2f;
    [SerializeField] float stressReductionDelay = 5f;

    //ref to enemy manager
    EnemyManager enemyManager;

    // B U F F S
    public int hits = 0;
    public bool trackHits = false;

    public bool currencyIncrease = false;
    public bool currencyPickedUp = false;


    [SerializeField]
    public CameraEffect CE;

    // S P E E D
    public float Speed = 8f;

    // A T T A C K
    public int Damage;


    public void Update()
    {
        CheckForHealthPickup();

        timeSinceLastHit += Time.deltaTime;
        ReduceStressOverTime(stressReductionRate);
    }

    public void Start()
    {
        //health = 3;
        stress = 0;
        reduceStressAmount = 1;
        //FindEnemyManager();
    }
    private void OnLevelWasLoaded(int level)
    {
        //FindEnemyManager();
    }

    void FindEnemyManager()
    {
        GameObject tempEm = GameObject.Find("EnemyManager");
        enemyManager = tempEm.GetComponent<EnemyManager>();
    }

    public void PickupHealth()
    {
        healthPickups++;
    }

    #region Health
    //public int GetHealth()
    //{
    //    return health;
    //}

    //public int GetMaxHealth()
    //{
    //    return maxHealth;
    //}

    //public void TakeDamage(int damage)
    //{
    //    health -= damage;
    //    if (trackHits) { hits++; }
    //}

    //public void GainHealth(int increase)
    //{
    //    health += increase;
    //}


    //bool usable;
    //public void UseHealth()
    //{
    //    usable = false;
    //    if (health < maxHealth)
    //    {
    //        healthPickups--;
    //        usable = true;
    //    }

    //    if (usable) { GainHealth(1); }
    //}

    //public void PlayerHasBeenHit()
    //{
    //    canTakeDamage = false;
    //    StartCoroutine(DamageCooldown());
    //    // Reduce player's health when colliding with an enemy
    //    ManageDamage(1);
    //}

    //private IEnumerator DamageCooldown()
    //{
    //    yield return new WaitForSeconds(damageCooldownDuration);
    //    canTakeDamage = true; // Allow taking damage again after cooldown
    //}

    //void ManageDamage(int damage)
    //{
    //    if (GetHealth() > 0)
    //    {
    //        TakeDamage(damage);

    //        MusicManager.instance.SetLowPassCutoffBasedOnHealth((float)GetHealth() / (float)GetMaxHealth());
    //        canTakeDamage = true;
    //        CE.ShakeCamera();
    //        // Check for player death
    //        if (GetHealth() == 0)
    //        {
    //            CE.StopCameraShake();
    //            //OnGameOver();
    //        }

    //    }
    //}
    #endregion

    public float GetStress()
    {
        return stress;
    }

    public int GetMaxStress()
    {
        return maxStress;
    }

    #region S T A M I N A

    public void IncreaseStress(int damage)//Take Damage
    {
        stress += damage;
        if (trackHits) { hits++; }
    }

    public void DecreaseStress(float decrease)//lower stress | Gain Health
    {
        if(stress > 0)
        {
            stress -= decrease;
        }
    }

    bool usable;
    public void UseHealth()
    {
        usable = false;
        if (stress < maxStress)
        {
            healthPickups--;
            usable = true;
        }

        if (usable) { DecreaseStress(reduceStressAmount); }
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
        if (GetStress() < GetMaxStress())
        {
            IncreaseStress(damage);

            MusicManager.instance.SetLowPassCutoffBasedOnHealth((float)GetStress() / (float)GetMaxStress());
            canTakeDamage = true;
            CE.ShakeCamera();
            // Check for player death
            if (GetStress() == 0)
            {
                CE.StopCameraShake();
                //OnGameOver();
            }

        }
    }

    public void ReduceStressOverTime(float stressReductionRate)
    {
        // If the player hasn't been hit for a set amount of time, gradually reduce stress
        if (timeSinceLastHit >= stressReductionDelay)
        {
            DecreaseStress(stressReductionRate * Time.deltaTime);
        }
    }

    #endregion

    internal void CheckForHealthPickup()
    {
        if (Input.GetButtonDown("UseHealth"))
        {
            Debug.Log("Player hit: UseHealth | E key");
            UseHealth();
        }
    }


    bool increased = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Currency"))
        {
            if (currencyIncrease)
            {
                increased = false;
                Currancy++; // add additional if buff is active
                increased = true;
            }
        }
    }

    
}
