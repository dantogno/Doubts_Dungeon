using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Audio;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    AudioSource musicAS;
    AudioLowPassFilter lpf;
    float currentCutoff;
    float targetCutoff;
    [SerializeField]
    float decayRate;

    bool targetIsHigher;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        musicAS = GetComponent<AudioSource>();
        lpf = GetComponent<AudioLowPassFilter>();
        currentCutoff = 22000;
        targetCutoff = 22000;
    }

    // Update is called once per frame
    void Update()
    {
        lpf.cutoffFrequency = currentCutoff;
        if(targetIsHigher && currentCutoff < targetCutoff )
        {
            currentCutoff += decayRate;
        }
        if(!targetIsHigher && currentCutoff > targetCutoff)
        {
            currentCutoff -= decayRate;
        }
    }

    public void SetLowPassCutoffBasedOnHealth(float healthPercent)
    {
        targetCutoff = 22000*ConvertToLogarithmicScale(healthPercent);
        if(currentCutoff > targetCutoff )
        {
            targetIsHigher = false;
        }
        if(currentCutoff < targetCutoff )
        {
            targetIsHigher = true;
        }
    }

    float ConvertToLogarithmicScale(float linearValue)
    {
        // Define your logarithmic scale base (e.g., 10 for base-10 logarithm)
        double logarithmicBase = 10.0;

        // Convert linear value to logarithmic scale
        // Using Math.Log function with the specified logarithmic base
        double logarithmicResult = Math.Log(1 + linearValue, logarithmicBase);

        return (float)logarithmicResult;
    }
}
