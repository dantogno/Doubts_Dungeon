using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Needed!
public class DetectInput : MonoBehaviour
{
    PlayerMovementNew moveScript;
    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<PlayerMovementNew>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || Input.GetMouseButtonDown(0))
        {
            moveScript.PlayerInputMode = InputType.Keyboard;
        }
        if(Input.GetAxisRaw("LSHorizontal") != 0 || Input.GetAxisRaw("RSHorizontal") != 0)
        {
            moveScript.PlayerInputMode = InputType.Controller;
        }
    }

}
