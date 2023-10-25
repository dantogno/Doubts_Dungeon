using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaScript : MonoBehaviour
{
    public Slider staminaBar;

    //port over to Player
    private int maxStamina = 100;
    private float currentStamina;

    float staminaRecoveryRate = 10;

    public static StaminaScript instance;

    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;

        staminaBar.value = maxStamina;
    }

    public bool UseStamina(float amount)
    {
        if(currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;
            return true;
        }
        else 
        {
            Debug.Log("Not enough stamina");
            return false; 
        }
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public void RecoverStamina()
    {
        if(currentStamina< maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            staminaBar.value = currentStamina;
        }
    }
}
