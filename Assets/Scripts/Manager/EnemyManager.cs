using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
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


    [Header("Enemies")]
    [SerializeField] GameObject tallEnemyPrefab;
    [SerializeField] GameObject shortEnemyPrefab;
    public List<GameObject> enemies = new List<GameObject>();

    [SerializeField] int minEnemyVal;
    [SerializeField] int maxEnemyVal;

    [Header("Enemy State")]
    public EnemyState EnemyState = new EnemyState();
    [SerializeField] private int WaveNumbers;
    [SerializeField] int roundEnemies;
    [SerializeField] public int NumOfWaves;
    [SerializeField] public int CurrentWave;

    public PortalState EndPortal;

    public bool WavesCompleted;

    [SerializeField] int DifficultyLevel = 1;
    [SerializeField] int DifficultyDuration = 30;//30 seconds


    [Header("Spawn Values")]
    [SerializeField] int minSpawnNum;
    [SerializeField] int AmountToChangeSpawns;
    public List<GameObject> spawnpoints = new List<GameObject>();


    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float minDistanceBetweenEnemies = 6f;
    [SerializeField] Vector3 enemyPosition;

    

    private NavMeshSurface navMeshSurface;


    //Timer
    [Header("Timer")]
    
    [SerializeField] public float CurrentTime;
    [SerializeField] bool TimerOn = false; 
    [SerializeField] public bool TimerCompleted = false;

    [SerializeField] float SurvivalTimeLimit = 100f;

    [SerializeField] TextMeshProUGUI TimerTxt;
    string MinuiteSecondFormat;


    public TextMeshProUGUI text;

    #region Highscore
    [SerializeField] TextMeshProUGUI HighscoreText;

    bool NewHighscore = false;

    int AllScores;
    int ScoresBeat = 0;

    private string HighscoreString;
    private float HighscoreInt;
    #endregion

    void Start()
    {
        //Get DontDestroy Components
        Canvas canvas = FindObjectOfType<Canvas>();
        Transform textTransform = canvas.transform.Find("TxtDisplay");
        TimerTxt = textTransform.GetComponent<TextMeshProUGUI>();

        text = textTransform.GetComponent<TextMeshProUGUI>();

        Transform gameoverScreen = canvas.transform.Find("GameOverScreen");
        Transform highscoreTransform = gameoverScreen.transform.Find("ScoreText");
        HighscoreText = highscoreTransform.GetComponent<TextMeshProUGUI>();

        CurrentTime = SurvivalTimeLimit;


        RandomizeSpawnPoints();

        Instance = this;
        if(EnemyState == EnemyState.Waves)
        {
            WavesCompleted = false;
            NumOfWaves = WaveNumbers;
            CurrentWave = 0;

            // Spawn and place enemies
            SpawnWave();
        }
        
        if(EnemyState == EnemyState.Survival)
        {
            TimerOn = true;
            TimerCompleted = false;
            StartSurvival();
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemyComponent = enemies[i].GetComponent<Enemy>();
            if (enemyComponent != null)
                enemyComponent.OnEnemyDestroyed += EnemyDestroyedHandler;
        }

    }

    private void RandomizeSpawnPoints()
    {
        for (int i = 0; i < AmountToChangeSpawns; i++)
        {
            int inty = Random.Range(minSpawnNum, spawnpoints.Count);

            spawnpoints.RemoveAt(inty);
        }
    }


    private void SpawnWave()
    {
        if(CurrentWave <= NumOfWaves)
        {
            SpawnAndPlaceEnemies();
        }
        else
        {
            return;
        }
        
    }

    private void StartSurvival()
    {
        SpawnAndPlaceEnemies();
    }

    private void DifficultyIncreassed()
    {
        maxEnemyVal += (maxEnemyVal + (maxEnemyVal / (30 * DifficultyLevel)));
    }

    private void Update()
    {
        //CheckForClearedRoom();
        if (enemies != null || enemies.Count == 0) 
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
            else if (TimerCompleted)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    GameObject enemy = enemies[i];
                    Enemy E = enemy.GetComponent<Enemy>();
                    if (E != null)
                    {
                        KillEnemy(E);  // Assuming this removes the enemy from the list
                        i--;  // Decrement index to account for removed enemy
                    }
                }

                // Clear the enemies list
                enemies.Clear();

                // Set timerCompleted to true to stop enemies from attacking
                TimerCompleted = true;

                EndPortal.PortalRef.SetActive(true);
                EndPortal.PortalRefCollider.enabled = true;
            }
        }
    }

    private void KillEnemy(Enemy enemy)
    {
        enemy.KillEnemy(enemy);
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

            if (EnemyState == EnemyState.Waves && !WavesCompleted)
            {
                SpawnWave();

                CheckForCompletedWaves();
            }
            else if (EnemyState == EnemyState.Waves && WavesCompleted) 
            { 
                text.text = "The waves are cleared!";
                EndPortal.PortalRef.SetActive(true);
                EndPortal.PortalRefCollider.enabled = true;
            }
            else if (EnemyState == EnemyState.Survival && TimerOn == true)
            {
                SpawnAndPlaceEnemies();
            }
            
        }
        //CheckForClearedRoom();
    }

    private void CheckForClearedRoom()
    {
        if (enemies.Count == 0)
        {
            text.text = "The room is cleared!";
        }
    }

    
    private void CheckForCompletedWaves()
    {
        if (CurrentWave >= NumOfWaves)
        {
            
            WavesCompleted = true;
            text.text = "Last Wave";
        }
    }

    #region Timer

    

    private void RunTimer()
    {
        CurrentTime -= Time.deltaTime;
        if(CurrentTime <= 0)
        {
            TimerOn = false;
            TimerCompleted = true;
            CurrentTime = 0;
            Debug.Log("Time's up!");

            TimerTxt.text = "You have survived";
        }
        ConvertToMinutes(CurrentTime);
    }

    int minutes, seconds, totalSeconds;
    private void ConvertToMinutes(float time)
    {
        minutes = Mathf.FloorToInt(time / 60);
        seconds = Mathf.FloorToInt(time % 60);

        totalSeconds = minutes * 60 + seconds; // Convert minutes back to seconds and add to remaining seconds

        if (SurvivalTimeLimit - totalSeconds >= (DifficultyDuration * DifficultyLevel))
        {
            DifficultyLevel++;
            DifficultyIncreassed();
        }

        //TimerTxt.text = $"Difficulty: {DifficultyLevel} |{minutes:D2}:{seconds:D2}";
        if(!TimerCompleted)
            TimerTxt.text = $"{minutes:D2}:{seconds:D2}";

        MinuiteSecondFormat = $"Dificult {DifficultyLevel} | {minutes:D2}:{seconds:D2}";
    }

    public void StopTimer()
    {
        TimerOn = false;
        Debug.Log($"Time Reached: {MinuiteSecondFormat}");
        //CheckForHighscore();
        CurrentTime = 0;
    }

    #endregion

    #region Highscore


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
