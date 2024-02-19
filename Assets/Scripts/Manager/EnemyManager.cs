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
    public static EnemyManager Instance;

    public EnemyState EnemyState = new EnemyState();

    public GameObject tallEnemyPrefab;
    public int minEnemyVal;
    public int maxEnemyVal;

    public GameObject shortEnemyPrefab;
    public int numberOfEnemies = 5;
    public float minDistanceBetweenEnemies = 6f;
    private Vector3 enemyPosition;

    [SerializeField]
    private int WaveNumbers;

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
        Instance = this;
        if(EnemyState == EnemyState.Waves)
        {
            NumOfWaves = WaveNumbers;
            CurrentWave = 0;

            // Spawn and place enemies
            SpawnWave();
        }
        
        if(EnemyState == EnemyState.Survival)
        {
            TimerOn = true;
            StartSurvival();
        }

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

    public int DifficultyLevel = 1;
    public int DifficultyDuration = 30;//30 seconds
    public int CurrentDuration = 30;

    private void StartSurvival()
    {
        SpawnAndPlaceEnemies();
    }

    private void DifficultyIncreassed()
    {
        //Adjusting to only increase the max range as increassing both made it impossible to quickly
        //minEnemyVal += (minEnemyVal + (minEnemyVal / (25 * DifficultyLevel)));
        maxEnemyVal += (maxEnemyVal + (maxEnemyVal / (30 * DifficultyLevel)));
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
        if (EnemyState == EnemyState.Survival) 
        {
            if (TimerOn)
            {
                RunTimer();
            }
        }
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
            if (spawnpoints[i].active == true)
            {
                enemyPosition = spawnpoints[i].transform.position;
                //for each spawn point spawn a randome number of enemies
                int inty = Random.Range(minEnemyVal, maxEnemyVal);
                SpawnSetNumEnemies(inty, enemyPosition);
            }
           
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
    public string MinuiteSecondFormat;

    //public List<float> Scores;

    private void RunTimer()
    {
        CurrentTime += Time.deltaTime;
        ConvertToMinutes(CurrentTime);
    }

    int minutes, seconds, totalSeconds;
    private void ConvertToMinutes(float time)
    {
        minutes = Mathf.FloorToInt(time / 60);
        seconds = Mathf.FloorToInt(time % 60);

        totalSeconds = minutes * 60 + seconds; // Convert minutes back to seconds and add to remaining seconds

        if (totalSeconds >= CurrentDuration)
        {
            DifficultyLevel++;
            DifficultyIncreassed();

            CurrentDuration += CurrentDuration;
        }

        TimerTxt.text = $"Difficulty: {DifficultyLevel} |{minutes:D2}:{seconds:D2}";

        MinuiteSecondFormat = $"{minutes:D2}:{seconds:D2}";
    }

    public void StopTimer()
    {
        TimerOn = false;
        Debug.Log($"Time Reached: {MinuiteSecondFormat}");
        CheckForHighscore();
        CurrentTime = 0;
    }

    #endregion

    #region Highscore

    public Text HighscoreText;

    bool NewHighscore = false;

    int AllScores;
    int ScoresBeat = 0;

    public string HighscoreString;
    public float HighscoreInt;

    private void CheckForHighscore()
    {
        #region Old Format
        //if(Scores.Count == 0)
        //{
        //    Highscore = MinuiteSecondFormat;
        //    HighscoreText.text = $"New Highscore! {MinuiteSecondFormat}";
        //    Scores.Add(CurrentTime);
        //}
        //else
        //{
        //    Scores.Add(CurrentTime);

        //    AllScores = Scores.Count;

        //    foreach (float score in Scores)
        //    {

        //        if(score > CurrentTime)
        //        {
        //            NewHighscore = false;
        //        }
        //        else if (score < CurrentTime) 
        //        {
        //            Debug.Log($"Previous Score: {score} | Current Score {CurrentTime}");
        //            ScoresBeat++;
        //        }
        //    }

        //    if(ScoresBeat == AllScores)
        //    {
        //        NewHighscore = true;
        //    }


        //    if (NewHighscore)
        //    {
        //        HighscoreText.text = $"New Highscore! {CurrentTime}";
        //        Highscore = MinuiteSecondFormat;
        //    }
        //    else
        //    {
        //        HighscoreText.text = $"Highscore: {Highscore} | Current Score: {MinuiteSecondFormat}";
        //    }
        //}
        #endregion

        float PrefScore = PlayerPrefs.GetFloat("HighscoreFloat", 0);

        if (CurrentTime > PrefScore)//if current time is higher then highscore
        {
            //new highscore
            PlayerPrefs.SetFloat("HighscoreFloat", CurrentTime);
            HighscoreText.text = $"New Highscore! {MinuiteSecondFormat}";
        }
        else
        {
            int m = Mathf.FloorToInt(PrefScore / 60);
            int s = Mathf.FloorToInt(PrefScore % 60);

            HighscoreText.text = $"Highscore: {m:D2}:{s:D2} | Current Score: {MinuiteSecondFormat}";
        }

    }

    public void SaveFloat()
    {
        PlayerPrefs.SetFloat("HighscoreFloat", CurrentTime);
    }

    #endregion
}
