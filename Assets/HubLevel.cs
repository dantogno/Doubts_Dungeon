using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class HubLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject transitionManager;
    TransitionManager transitionManagerComponent;

    void Start()
    {
        transitionManager = GameObject.Find("TransitionManager");
        transitionManagerComponent = transitionManager.GetComponent<TransitionManager>();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (transitionManager == null || transitionManagerComponent == null)
        {
            transitionManager = GameObject.Find("TransitionManager");
            transitionManagerComponent = transitionManager.GetComponent<TransitionManager>();
        }
        
        transitionManagerComponent.StartRoom = false;
    }
}
