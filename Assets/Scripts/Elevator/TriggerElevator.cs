using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerElevator : MonoBehaviour
{
    [SerializeField]
    private MovePlatfrom platform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        while (other.gameObject.CompareTag("Player"))
        {
            platform.CanMove = true;
        }
    }
}
