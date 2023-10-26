using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            interact();
        }
    }

    void interact()
    {
        if(interactable != null)
        {
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
