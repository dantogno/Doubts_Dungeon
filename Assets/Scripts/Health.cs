using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthamount;
    private PlayerHealth playerref;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerref = other.GetComponent<PlayerHealth>();

            playerref.PickupHealth();

            Destroy(gameObject);
        }
    }
}
