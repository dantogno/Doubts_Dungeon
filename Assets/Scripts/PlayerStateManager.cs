using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState { Idle, Dodge, Sprint, Attack }
public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager instance;

    public ActionState ActionState { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckActionState()
    {
        switch (ActionState)
        {
            case ActionState.Dodge:
                Debug.Log("ActionState: Dodge");
                //cannot Attack
                break;
            case ActionState.Sprint:
                Debug.Log("ActionState: Sprint");
                //Cannot Attack
                break;
            case ActionState.Attack:
                Debug.Log("ActionState: Attack");
                //Pauses Movement
                break;
        }
    }
}
