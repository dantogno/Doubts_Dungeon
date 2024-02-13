using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    Interactable interactable;

    [SerializeField]
    private bool HasbeenInteracted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(HasbeenInteracted == false)
            {
                interact();
                
            }
            
        }
    }

    void interact()
    {
        if(interactable != null)
        {
            Debug.Log("Is Interacting");
            interactable.Interact();
            HasbeenInteracted = false;
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
