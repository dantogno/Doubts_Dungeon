using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { Controller, Keyboard }

public class GameManager : MonoBehaviour
{
    public static event Action OnPlayerDeath;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGameOver()
    {
        //set something on enemymanager
        EnemyManager.Instance.StopTimer();
        OnPlayerDeath?.Invoke();
    }

    private void OnEnable()
    {
        OnPlayerDeath += DisablePlayer;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= DisablePlayer;
    }

    public void DisablePlayer()
    {
        enabled = false;
    }
}
