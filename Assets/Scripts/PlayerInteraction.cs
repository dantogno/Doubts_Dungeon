using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    Interactable interactable;

    [SerializeField]
    private bool isTalking;

    [SerializeField]
    private bool isChest;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            
                interact();
                
            
            
        }
    }

    void interact()
    {
        if(interactable != null)
        {
            Debug.Log("Is Interacting");
            interactable.Interact();
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Interactable>(out Interactable script))
        {
            interactable = script;
            script.EnterTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable script))
        {
            interactable = null;
            script.ExitTrigger();
        }
    }

    

}
