using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.PlayerLoop;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent;
    public float EnemyPathingDelay = 0.2f;
    public float knockbackDistance = 2f;
    public float knockbackDuration = .5f;

    [SerializeField]
    private Animator animator;

    public float maxRange = 15f;

    private float PathUpdateDeadline = 1f;
    private Transform target;

    public int health = 5;
    public int maxhealth;

    public event Action<Enemy> OnEnemyDestroyed;
    public static event Action EnemyHit;

    private const string AttackTrigger = "Attack";
    private const string IsWalking = "IsWalking";
    private const string HitTrigger = "isHit";

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
       
    }

    void Start()
    {

        // Initialize target to the player
        target = GameObject.FindGameObjectWithTag("Player").transform;
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

    private void UpdatePath()
    {
        if (Time.time >= PathUpdateDeadline)
        {
            animator.SetBool(IsWalking, true); 
            Debug.Log("Updating Path");
            PathUpdateDeadline = Time.time + EnemyPathingDelay;
            Agent.SetDestination(target.position);
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

        if (collision.gameObject.CompareTag("Weapon"))
        {
            TakeDamage(1);
        }
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
        EnemyHit?.Invoke();
        health -= damage;


        // Check for player death
        if (health <= 0)
        {
            Die();
        }
    }

    private void NotifyEnemyDestroyed()
    {
        OnEnemyDestroyed?.Invoke(this);
    }

    void Die()
    {
        NotifyEnemyDestroyed();
        Destroy(gameObject);
    }
}
