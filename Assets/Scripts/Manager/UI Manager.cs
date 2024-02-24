using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Player player;
    public Image[] TotalHearts;
    public TextMeshProUGUI healthPickups;
    public int health;
    public int maxhealth;

    void Update()
    {
        health = player.GetHealth();
        maxhealth = player.GetMaxHealth();

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

        UpdateHealthPickups();
    }

    private void UpdateHealthPickups()
    {
        healthPickups.text = $"Stabalizers: {player.healthPickups}";
    }
}
