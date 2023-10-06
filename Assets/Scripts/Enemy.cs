using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public NavMeshAgent Agent;
    public float EnemyPathingDelay = 0.2f;

    public float knockbackDistance = 4f;
    public float knockbackDuration = 4f;

    private float PathUpdateDeadline = 1f;
    private Transform target;

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
        //FollowTarget();

        if (target != null)
        {
            UpdatePath();
        }
    }

    private void UpdatePath()
    {
        if (Time.time >= PathUpdateDeadline)
        {
            Debug.Log("Updating Path");
            PathUpdateDeadline = Time.time + EnemyPathingDelay;
            Agent.SetDestination(target.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate direction from the enemy to the player
            Vector3 direction = transform.position - collision.transform.position;
            direction.Normalize();

            // Calculate the knockback position
            Vector3 knockbackPosition = transform.position + direction * knockbackDistance;

            // Move the enemy to the knockback position over the knockback duration
            StartCoroutine(Knockback(knockbackPosition, knockbackDuration));
        }
    }

    private IEnumerator Knockback(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

}
