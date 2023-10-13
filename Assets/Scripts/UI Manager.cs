using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerMovement Player;
    public Image[] TotalHearts;
    public int health;
    public int maxhealth;

<<<<<<< Updated upstream
=======

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
>>>>>>> Stashed changes
    void Update()
    {
        health = Player.health;
        maxhealth = Player.maxhealth;

        for (int i = 0; i < TotalHearts.Length; i++)
        {
            if (i < health)
            {
                TotalHearts[i].color = new Color(100f, 100f, 100f, 1f);

            }
            else
            {
                TotalHearts[i].color = new Color(100f, 100f, 100f, .1f);


            }
            if (i < maxhealth)
            {
                TotalHearts[i].enabled = true;
            }
            else
            {
                TotalHearts[i].enabled = false;
            }
        }
    }
}
