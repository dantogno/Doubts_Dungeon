using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectInput : MonoBehaviour
{
    PlayerMovement moveScript;
    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || Input.GetMouseButtonDown(0))
        {
            moveScript.usingController = false;
        }
        if(Input.GetAxisRaw("LSHorizontal") != 0 || Input.GetAxisRaw("RSHorizontal") != 0)
        {
            moveScript.usingController = true;
        }
    }

}
