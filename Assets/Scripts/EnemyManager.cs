using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.ShaderData;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EnemyManager : MonoBehaviour
{

    public GameObject tallEnemyPrefab;
    public int minEnemyVal;
    public int maxEnemyVal;

    public GameObject shortEnemyPrefab;
    public int numberOfEnemies = 5;
    public float minDistanceBetweenEnemies = 6f;
    private Vector3 enemyPosition;

    public int roundEnemies;

    private NavMeshSurface navMeshSurface;

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> spawnpoints = new List<GameObject>();

    public GameObject levelDoor;
    public Text text;
    public GameObject winFloor;

    public int NumOfWaves;
    public int CurrentWave;

    void Start()
    {

        NumOfWaves = 5;
        CurrentWave = 0;

        // Spawn and place enemies
        SpawnWave();


        //foreach (var enemy in enemies)
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemyComponent = enemies[i].GetComponent<Enemy>();
            if (enemyComponent != null)
                enemyComponent.OnEnemyDestroyed += EnemyDestroyedHandler;
        }

    }

    private void SpawnWave()
    {
        if(CurrentWave <= NumOfWaves)
        {
            SpawnAndPlaceEnemies();
        }
        
    }

    private void Update()
    {
        //CheckForClearedRoom();
        if(enemies != null || enemies.Count == 0) 
        {
            for(int i =0; i < enemies.Count; i++)
            {
                Enemy enemyComponent = enemies[i].GetComponent<Enemy>();
                if (enemyComponent != null)
                    enemyComponent.OnEnemyDestroyed += EnemyDestroyedHandler;
            }
            //foreach (var enemy in enemies)
            //{
            //    Enemy enemyComponent = enemy.GetComponent<Enemy>();
            //    if (enemyComponent != null)
            //        enemyComponent.OnEnemyDestroyed += EnemyDestroyedHandler;
            //}
            //Be careful with foreach statments
        }
        
    }


    private void SpawnAndPlaceEnemies()
    {
        CurrentWave++;
        text.text = $"Wave: {CurrentWave}/{NumOfWaves}";

        for (int i = 0; i < spawnpoints.Count; i++ )
        {
            enemyPosition = spawnpoints[i].transform.position;
            //for each spawn point spawn a randome number of enemies
           int inty =  Random.Range(minEnemyVal,maxEnemyVal);
            SpawnSetNumEnemies(inty, enemyPosition);
        }

       
    }

    public void SpawnSetNumEnemies(int num, Vector3 enemyPos)
    {
        for(int i = 0; i < num; i++)//Shift to randomize whihc enemy spawns 
        {
            roundEnemies++;

            GameObject enemyPrefab = Random.Range(0, 2) == 0 ? tallEnemyPrefab : shortEnemyPrefab;

            //Instantiate the enemy
            GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
            enemies.Add(enemy);
        }
    }

    private void EnemyDestroyedHandler(Enemy destroyedEnemy)
    {
        // Remove the destroyed enemy from the list
        enemies.Remove(destroyedEnemy.gameObject);

        if(enemies.Count < roundEnemies)
        {
            text.text = $"There are {enemies.Count} left";
        }
        

        // Check for cleared room when an enemy is destroyed
        if(enemies.Count == 0)
        {
            roundEnemies = 0;
            SpawnWave();

            CheckForCompletedWaves();
        }
        //CheckForClearedRoom();
    }

    private void CheckForClearedRoom()
    {
        if (enemies.Count == 0 && levelDoor != null)
        {
            text.text = "The room is cleared!";
            Destroy(levelDoor);
        }
    }

    private void CheckForCompletedWaves()
    {
        if (CurrentWave >= NumOfWaves && levelDoor != null)
        {
            text.text = "The waves are cleared!";
            Destroy(levelDoor);
        }
    }
}
