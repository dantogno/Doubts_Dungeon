using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmBehavior : MonoBehaviour
{
    public float detectionRange = 10f;
    public int maxGroupSize = 3;
    public float groupFormationRange = 5f;
    public float swarmSpeed = 5f;
    public float rotationSpeed = 2f;

    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();

    private Dictionary<GameObject, List<GameObject>> enemyGroups = new Dictionary<GameObject, List<GameObject>>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        // Initialize enemy groups
        foreach (GameObject enemy in enemies)
        {
            enemyGroups.Add(enemy, new List<GameObject>());
        }
    }

    void Update()
    {
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < detectionRange)
            {
                FormGroup(enemy);
            }
        }

        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        foreach (GameObject enemy in enemies)
        {
            if (IsGrouped(enemy))
            {
                Vector3 direction = (player.transform.position - enemy.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                enemy.transform.position += enemy.transform.forward * swarmSpeed * Time.deltaTime;
            }
        }
    }

    void FormGroup(GameObject enemy)
    {
        if (IsGrouped(enemy))
            return;

        List<GameObject> nearbyEnemies = new List<GameObject>();
        foreach (GameObject otherEnemy in enemies)
        {
            if (otherEnemy != enemy && Vector3.Distance(enemy.transform.position, otherEnemy.transform.position) < groupFormationRange)
            {
                nearbyEnemies.Add(otherEnemy);
            }
        }

        if (nearbyEnemies.Count < maxGroupSize)
        {
            JoinGroup(enemy, player);
            foreach (GameObject nearbyEnemy in nearbyEnemies)
            {
                JoinGroup(nearbyEnemy, player);
            }
        }
    }

    bool IsGrouped(GameObject enemy)
    {
        return enemyGroups.ContainsKey(enemy) && enemyGroups[enemy].Count > 0;
    }

    void JoinGroup(GameObject enemy, GameObject leader)
    {
        if (!enemyGroups.ContainsKey(enemy))
            enemyGroups.Add(enemy, new List<GameObject>());

        if (!enemyGroups[enemy].Contains(leader))
            enemyGroups[enemy].Add(leader);
    }
}
