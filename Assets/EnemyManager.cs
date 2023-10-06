using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderData;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EnemyManager : MonoBehaviour
{
    public GameObject tallEnemyPrefab;
    public GameObject shortEnemyPrefab;
    public int numberOfEnemies = 5;
    public float minDistanceBetweenEnemies = 3f;

    private NavMeshSurface navMeshSurface;

    public List<GameObject> enemies = new List<GameObject>();

    public GameObject levelDoor;
    public GameObject winFloor;

    void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();

        // Spawn and place enemies
        SpawnAndPlaceEnemies();

        foreach (var enemy in enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
                enemyComponent.OnEnemyDestroyed += EnemyDestroyedHandler;
        }

    }

    private void Update()
    {
        CheckForClearedRoom();
    }

    private void SpawnAndPlaceEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject enemyPrefab = Random.Range(0, 2) == 0 ? tallEnemyPrefab : shortEnemyPrefab;
            Vector3 enemyPosition = GetRandomValidPosition();

            // Instantiate the enemy
            GameObject enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            enemies.Add(enemy);
        }
    }

    private Vector3 GetRandomValidPosition()
    {
        NavMeshHit hit;
        Vector3 randomPosition = Vector3.zero;

        while (true)
        {
            float x = Random.Range(-50f, 50f);
            float z = Random.Range(-50f, 50f);

            randomPosition = new Vector3(x, 0f, z);


            if (!IsPositionWithinRestrictedArea(randomPosition, winFloor))
            {
                if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
                {
                    bool validPosition = true;
                    foreach (var enemy in enemies)
                    {
                        float distance = Vector3.Distance(enemy.transform.position, randomPosition);
                        if (distance < minDistanceBetweenEnemies)
                        {
                            validPosition = false;
                            break;
                        }
                    }

                    if (validPosition)
                        return hit.position;
                }
            }
        }
    }
        
    
    private bool IsPositionWithinRestrictedArea(Vector3 position, GameObject restrictedAreaObject)
    {
        if (restrictedAreaObject == null)
        {
            // If no restricted area defined, consider position as valid
            return false;
        }

        Collider restrictedCollider = restrictedAreaObject.GetComponent<Collider>();
        if (restrictedCollider != null)
        {
            // Check if the position is within the collider bounds
            return restrictedCollider.bounds.Contains(position);
        }
        return false; // If no valid collider found, consider position as valid
    }

    private void EnemyDestroyedHandler(Enemy destroyedEnemy)
    {
        // Remove the destroyed enemy from the list
        enemies.Remove(destroyedEnemy.gameObject);

        // Check for cleared room when an enemy is destroyed
        CheckForClearedRoom();
    }

    private void CheckForClearedRoom()
    {
        if (enemies.Count == 0 && levelDoor != null)
        {
            Destroy(levelDoor);
        }
    }
}
