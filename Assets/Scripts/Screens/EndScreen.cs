using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject endscreenUI;

    public void LoadMenu() 
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    
    
    }
}
