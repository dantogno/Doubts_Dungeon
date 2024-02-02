using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerHealth PH;
    // Start is called before the first frame update
    void Start()
    {
        PH = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (PH.canTakeDamage)
            {
                PH.PlayerHasBeenHit();
            }

        }
        else if (collision.gameObject.CompareTag("Trap"))
        {
            if (PH.canTakeDamage)
            {
                PH.PlayerHasBeenHit();
            }
        }
    }
}
