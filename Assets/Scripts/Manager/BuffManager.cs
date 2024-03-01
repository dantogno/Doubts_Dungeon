using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField]
    Player player;

    // D U R A T I O N
    [SerializeField] float currentTime = 0;
    [SerializeField] bool DurationBuffOn = false;

    // A D R E N A L I N
    [SerializeField] float adrenalinDuration = 120;
    [SerializeField] bool adrenalinOn = false;

    // E N D O R P H I N S
    [SerializeField] int endorphinsHitLimit = 3;
    [SerializeField] bool endorphinsActive = false;
    [SerializeField] bool endorphinsOn = false;

    // D O P A M I N E
    [SerializeField] int dopamineDropIncrease = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayablePlayer = GameObject.Find("PlayablePlayer");
        Transform Menty = PlayablePlayer.transform.Find("MentyPlayer");//Breaking
        player = Menty.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        TestBuffs();
    }

    private void FixedUpdate()
    {
        if (DurationBuffOn) { currentTime += Time.deltaTime; }
        else { currentTime = 0; }

        if (adrenalinOn) { Adrenalin(); }
        if (endorphinsOn) {  Endorphins(); }
    }

    //Stabilizer, Adrenalin, Histamine, Acetylcholine, Endorphins, Noradrenaline, Dopamine
    public void UseBuff(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Stabilizer:
                Stabilizer();
                break;
            case ItemType.Adrenalin:
                Adrenalin();
                break;
            case ItemType.Histamine:
                Histamine();
                break;
            case ItemType.Acetylcholine://Permanent
                Acetylcholine();
                break;
            case ItemType.Endorphins:
                Endorphins();
                break;
            case ItemType.Noradrenaline://Perminant
                Noradrenaline();
                break;
            case ItemType.Dopamine://Perminent
                Dopamine();
                break;
        }
    }

    public void ResetBuffs()
    {
        player.currencyIncrease = false;
    }

    void Stabilizer()//1
    {
        Debug.Log("Stabilizer Used");
        player.healthPickups++; 
    } 

    void Adrenalin()//2
    {
        Debug.Log("Adrenalin Active");
        adrenalinOn = true;
        //Mario Super star (invincible for a durration)
        DurationBuffOn = true;
        if(adrenalinDuration >= currentTime)
        {
            player.canTakeDamage = false;
            //Add a visual que like flashing hearts
        }
        else
        {
            adrenalinOn = false;
            player.canTakeDamage = true;
            DurationBuffOn = false;
            Debug.Log("Adrenalin In-Active");
        }

    }

    void Endorphins()//5
    {
        Debug.Log("Endorphins Active");

        endorphinsOn = true;
        //Shield | Stays for a set number of hits
        endorphinsActive = true;
        player.trackHits = true;
        player.canTakeDamage = false;

        if (player.hits >= endorphinsHitLimit)
        {
            endorphinsOn = false;
            endorphinsActive = false;
            player.trackHits = false;
            player.canTakeDamage = true;
            player.hits = 0;
            Debug.Log("Endorphins In-Active");
        }

    }

    //under construction
    void Histamine()//3
    {
        Debug.Log("Histamine Active");
        //Lower enemy health in a room | One Time
    }

    //under construction
    void Acetylcholine()//4
    {
        Debug.Log("Acetylcholine Active");
        //Increased Damage | Permanent
        //player.UpdateEnemyManager(2);

    }
    

    //under construction
    void Noradrenaline()//6
    {
        Debug.Log("Noradrenaline Active");
        //Increased Speed | Perminant

    }

    void Dopamine()//7
    {
        Debug.Log("Dopamine Active");
        //Increases Currency Drop | Perminent
        player.currencyIncrease = true;
        //Player manages this
    }

    public void TestBuffs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))//Stabilizer
        {
            Stabilizer();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))//Adrenalin
        {
            Adrenalin();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))//Histamine
        {
            Histamine();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))//Acetylcholine
        {
            Acetylcholine();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))//Endorphins
        {
            Endorphins();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))//Noradrenaline
        {
            Noradrenaline();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))//Dopamine
        {
            Dopamine();
        }
    }


}
