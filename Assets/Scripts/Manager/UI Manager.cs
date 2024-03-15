using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Player player;
    //public Image[] TotalHearts;
    //public int health;
    //public int maxhealth;
    public TextMeshProUGUI healthPickups;
    public TextMeshProUGUI currencyDisplay;

    public Slider stressSlider;
    

    void Update()
    {
        #region Health
        //health = player.GetHealth();
        //maxhealth = player.GetMaxHealth();

        //for (int i = 0; i < TotalHearts.Length; i++)
        //{
        //    if (i < health)
        //    {
        //        TotalHearts[i].color = new Color(100f, 100f, 100f, 1f);

        //    }
        //    else
        //    {
        //        TotalHearts[i].color = new Color(100f, 100f, 100f, .1f);


        //    }
        //    if (i < maxhealth)
        //    {
        //        TotalHearts[i].enabled = true;
        //    }
        //    else
        //    {
        //        TotalHearts[i].enabled = false;
        //    }
        //}
        #endregion

        UpdateStressSlider();
        UpdateHealthPickups();
        UpdateCurancyDisplay();
    }

    private void UpdateStressSlider()
    {
        stressSlider.value = player.GetStress();
        stressSlider.maxValue = player.GetMaxStress();
    }

    private void UpdateHealthPickups()
    {
        healthPickups.text = $": {player.healthPickups}";
    }

    private void UpdateCurancyDisplay()
    {
        currencyDisplay.text = $": {player.Currancy}";
    }
}
