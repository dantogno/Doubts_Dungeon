using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    //Using ID enables this to work even if two items have the same name
    public string objectID;

    private void Awake()
    {
        objectID = name + transform.position.ToString();
    }

    void Start()
    {
        //This Enables objects to not duplicate when returning to a previous space
        DontDestroy[] protectedObjects = Object.FindObjectsOfType<DontDestroy>();

        //For all of the objects that have the DontDestroy Script
        for (int i =0; i < protectedObjects.Length; i++)
        {
            if (protectedObjects[i] != this)
            {
                if (protectedObjects[i].objectID == objectID)
                {
                    Destroy(gameObject);
                }
                
            }
        }

        DontDestroyOnLoad(gameObject);
    }

}
