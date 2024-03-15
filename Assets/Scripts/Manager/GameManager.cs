using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { Controller, Keyboard }

public class GameManager : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    public Player P;


    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayablePlayer = GameObject.Find("PlayablePlayer");
        Transform MentyPlayer = PlayablePlayer.transform.Find("MentyPlayer");//Breaking
        P = MentyPlayer.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(P.health == 0)
        //{
        //    OnGameOver();
        //}
        if (P.stress >= P.maxStress)
        {
            OnGameOver();
        }
    }

    public void OnGameOver()
    {
        //set something on enemymanager
        //EnemyManager.Instance.StopTimer();
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
