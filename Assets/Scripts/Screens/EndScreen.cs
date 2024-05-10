using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject endscreenUI;

    public void Start()
    {
        //transitionManager = GameObject.Find("TransitionManager");

    }

    public void LoadMenu() 
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
