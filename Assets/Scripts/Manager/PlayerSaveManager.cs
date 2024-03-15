using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveManager : MonoBehaviour
{
    [SerializeField] Player SavedPlayer;
    [SerializeField] Player CurrentPlayer;


    // Start is called before the first frame update
    void Start()
    {
        GetCurrentPlayer();
    }

    private void OnLevelWasLoaded(int level)
    {
        GetCurrentPlayer();

        SetValues();
    }

    // Update is called once per frame
    void Update()
    {
        SaveValues();
    }

    public void GetCurrentPlayer()
    {
        GameObject PlayablePlayer = GameObject.Find("PlayablePlayer");
        Transform Menty = PlayablePlayer.transform.Find("MentyPlayer");//Breaking
        CurrentPlayer = Menty.GetComponent<Player>();
    }

    public void SaveValues()
    {
        //Heath
        //SavedPlayer.health = CurrentPlayer.health;
        //Stress
        SavedPlayer.stress = CurrentPlayer.stress;
        //Health Pickups
        SavedPlayer.healthPickups = CurrentPlayer.healthPickups;
        //Currenty
        SavedPlayer.Currancy = CurrentPlayer.Currancy;
        //Inventory
    }

    public void SetValues()
    {
        if(SavedPlayer != null) 
        {
            //Heath
            //CurrentPlayer.health = SavedPlayer.health;
            //Stress
            CurrentPlayer.stress = SavedPlayer.stress;
            //Health Pickups
            CurrentPlayer.healthPickups = SavedPlayer.healthPickups;
            //Currenty
            CurrentPlayer.Currancy = SavedPlayer.Currancy;
            //Inventory
        }

    }

    public void ClearSaveValues()
    {
        //Heath
        //SavedPlayer.health = 3;

        //Stress
        SavedPlayer.stress = 0;
        //Health Pickups
        SavedPlayer.healthPickups = 0;
        //Currenty
        SavedPlayer.Currancy = 0;

        SetValues();
    }

    
}
