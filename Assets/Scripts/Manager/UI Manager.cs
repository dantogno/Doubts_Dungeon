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
    public TextMeshProUGUI currencyDisplay;
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
        UpdateCurancyDisplay();
    }

    private void UpdateHealthPickups()
    {
        healthPickups.text = $"Stabilizers: {player.healthPickups}";
    }

    private void UpdateCurancyDisplay()
    {
        currencyDisplay.text = $"Bad Energy: {player.Currancy}";
    }
}
