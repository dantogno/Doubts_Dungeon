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

        //For all of the objects that have the DontDestroy Script
        for(int i =0; i < Object.FindObjectsOfType<DontDestroy>().Length; i++)
        {
            if (Object.FindObjectsOfType<DontDestroy>()[i] != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
