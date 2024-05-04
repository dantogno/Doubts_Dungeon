using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.PlayerLoop;
using UnityEngine.AI;
using UnityEngine.VFX;
using System;

public class Enemy : MonoBehaviour
{
    // M O V E M E N T
    public NavMeshAgent Agent;
    public float EnemyPathingDelay = 0.2f;
    public float knockbackDistance = 2f;
    public float knockbackDuration = .5f;


    private float PathUpdateDeadline = 1f;
    private Transform target;

    [SerializeField]
    private Transform spawnpos;

    [SerializeField]
    public AudioSource enemySource;
    public AudioClip deathSound;
    public float volume = 1f;


    // V I S U A L S 
    [SerializeField]
    private DamageFlash enemydmg;

    [SerializeField]
    private float despawntime = 1.2f;

    private bool deaddrop;

    [SerializeField]
    private Animator animator;

    public float maxRange = 15f;

    // H E A L T H
    public int health = 5;
    public int maxhealth;
    public int DamageTaken;

    [SerializeField]
    private Transform YOffset;

    [SerializeField]
    private bool CanBeKnockedBack = true;

    public event Action<Enemy> OnEnemyDestroyed;
    public static event Action EnemyHit;

    private const string AttackTrigger = "Attack";
    private const string IsWalking = "IsWalking";
    private const string IsDead = "IsDead";
    private const string HitTrigger = "isHit";

    //Dissolve Shader: Variables
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;

    public Currancy orbref;

    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinnedMaterials;

    //player ref
    PlayerMelee playerMelee;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        StartCoroutine(DelayedSetup());

        // Initialize target to the player
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Dissolve Shader: Whenever the skinned mesh changes, this if function sets that new material in the array
        if (skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;

        DamageTaken = 1;
    }

    private IEnumerator DelayedSetup()
    {
        yield return new WaitForSeconds(0.1f); // Adjust delay time as needed
        UpdatePath(); // Call method to set destination after a delay
    }

    private void UpdatePath()
    {
        if (Agent != null && Agent.isActiveAndEnabled)
        {
            animator.SetBool(IsWalking, true);

            if(target != null)
            {
                Agent.SetDestination(target.position);
            }

        }
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= maxRange)  // Adjust 'maxRange' to your desired maximum targeting range
            {
                UpdatePath();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger(AttackTrigger);

            // Calculate direction from the enemy to the player
            Vector3 direction = transform.position - collision.transform.position;
            direction.Normalize();

            // Calculate the knockback position
            Vector3 knockbackPosition = transform.position + direction * knockbackDistance;

            // Move the enemy to the knockback position over the knockback duration
            StartCoroutine(Knockback(knockbackPosition, knockbackDuration));
        }

        //taking damage from projectile
        if (collision.gameObject.CompareTag("Weapon"))
        {
            if (CanBeKnockedBack == true)
            {
                animator.SetTrigger(HitTrigger);

                // Calculate direction from the enemy to the player
                Vector3 direction = transform.position - collision.transform.position;
                direction.Normalize();

                // Calculate the knockback position
                Vector3 knockbackPosition = transform.position + direction * knockbackDistance;

                // Move the enemy to the knockback position over the knockback duration
                StartCoroutine(Knockback(knockbackPosition, knockbackDuration));
            }

            TakeDamage(DamageTaken);
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        //for taking damage from melee
        if (other.gameObject.CompareTag("Melee"))
        {
            playerMelee = other.gameObject.GetComponentInParent<PlayerMelee>();

            if (playerMelee != null &&  playerMelee.Attacked)
            {
                //hit behavior
                if (CanBeKnockedBack == true)
                {
                    animator.SetTrigger(HitTrigger);

                    // Calculate direction from the enemy to the player
                    Vector3 direction = transform.position - other.transform.position;
                    direction.Normalize();

                    // Calculate the knockback position
                    Vector3 knockbackPosition = transform.position + direction * knockbackDistance;

                    // Move the enemy to the knockback position over the knockback duration
                    StartCoroutine(Knockback(knockbackPosition, knockbackDuration));
                }
                UnityEngine.Debug.Log($"{this.gameObject.name} MELEE HIT!");
                TakeDamage(DamageTaken * 2);

                playerMelee.Performed();
            }

            
        }

        playerMelee = null;
    }


    private IEnumerator Knockback(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        float initialY = transform.position.y;

        while (elapsedTime < duration)
        {
            float newY = initialY;  // Maintain the initial y position
            transform.position = new Vector3(
                Mathf.Lerp(initialPosition.x, targetPosition.x, elapsedTime / duration),
                newY,
                Mathf.Lerp(initialPosition.z, targetPosition.z, elapsedTime / duration)
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, initialY, targetPosition.z);  // Set final position with correct y
    }

    public void TakeDamage(int damage)
    {
        enemydmg.Flash();
        health -= damage;


        // Check for player death
        if (health == 0)
        {
            deaddrop = true;
            StartCoroutine(Dissolve()); 
            Die();
        }
    }

    public void KillEnemy(Enemy enemy) 
    {
        enemy.deaddrop = true;
        enemy.Die();
        enemy.StartCoroutine(Dissolve());
    }

    private void NotifyEnemyDestroyed()
    {
        OnEnemyDestroyed?.Invoke(this);
    }

    void Die()
    {
        if(deaddrop == true)
        {
            orbref.transform.position = new Vector3(gameObject.transform.position.x, YOffset.position.y, gameObject.transform.position.z);
            orbref.SpawnObject();
        }
       
        NotifyEnemyDestroyed();
        Agent.speed = 0;
        animator.SetTrigger(IsDead);
        enemySource.PlayOneShot(deathSound, volume);
        //gameObject.SetActive(false);
        //DestroyImmediate(gameObject, true);
    }

    public IEnumerator Dissolve()
    {
        if (VFXGraph != null)
            VFXGraph.Play();

        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;

            while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }

        gameObject.SetActive(false);
    }
}
