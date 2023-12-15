using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthamount;
    private PlayerMovement playerref;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerref = other.GetComponent<PlayerMovement>();

            if(playerref.health < 3)
            {
                playerref.health += healthamount;

                Destroy(gameObject);
            }
        }
    }
}
