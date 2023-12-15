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

public enum EnemyState
{
    Waves,
    Survival
}

public class EnemyManager : MonoBehaviour
{

    public EnemyState EnemyState = new EnemyState();

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
        if(EnemyState == EnemyState.Waves)
        {
            NumOfWaves = 5;
            CurrentWave = 0;

            // Spawn and place enemies
            SpawnWave();
        }
        
        if(EnemyState == EnemyState.Survival)
        {
            TimerOn = true;
            StartSurvival();
        }


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

    private void StartSurvival()
    {
        SpawnAndPlaceEnemies();
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
        }
    }

    private void FixedUpdate()
    {
        if (EnemyState == EnemyState.Survival) { RunTimer(); }
    }

    private void SpawnAndPlaceEnemies()
    {
        if(EnemyState == EnemyState.Waves) 
        {
            CurrentWave++;
            text.text = $"Wave: {CurrentWave}/{NumOfWaves}";
        }

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

        if(EnemyState == EnemyState.Waves)
        {
            if (enemies.Count < roundEnemies)
            {
                text.text = $"There are {enemies.Count} left";
            }
        }
        

        // Check for cleared room when an enemy is destroyed
        if(enemies.Count == 0)
        {
            roundEnemies = 0;

            if(EnemyState == EnemyState.Waves)
            {
                SpawnWave();

                CheckForCompletedWaves();
            }
            else if(EnemyState == EnemyState.Survival)
            {
                SpawnAndPlaceEnemies();
            }
            
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

    #region Timer

    public float CurrentTime;
    public bool TimerOn = false;

    public Text TimerTxt;

    public List<float> Scores;
    public float Highscore;

    private void RunTimer()
    {
        CurrentTime += Time.deltaTime;
        ConvertToMinutes(CurrentTime);
    }

    int minuites, seconds;
    private void ConvertToMinutes(float time)
    {
        minuites = Mathf.FloorToInt(CurrentTime / 60);
        seconds = Mathf.FloorToInt(CurrentTime % 60);

        TimerTxt.text = $"{minuites:D2}:{seconds:D2}";
    }

    private void StopTimer()
    {
        TimerOn = false;
        Debug.Log($"Time Reached: {CurrentTime}");
        CheckForHighscore();
        CurrentTime = 0;
    }

    #endregion

    #region Highscore
    bool NewHighscore = false;
    private void CheckForHighscore()
    {
        if(Scores.Count == 0)
        {
            Highscore = CurrentTime;
            Scores.Add(CurrentTime);
        }
        else
        {
            foreach(float score in Scores)
            {
                if (score < CurrentTime) 
                {
                    Debug.Log($"Previous Score: {score} | Current Score {CurrentTime}");
                    NewHighscore = true;
                    continue;
                }
                else
                {
                    NewHighscore = false;
                    break;
                }
            }

            Scores.Add(CurrentTime);

            if (NewHighscore)
            {
                Highscore = CurrentTime;
            }
            
        }
    }
    #endregion
}
