using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    Player P;
    // Start is called before the first frame update
    void Start()
    {
        P = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (P.canTakeDamage)
            {
                P.PlayerHasBeenHit();
            }

        }
        else if (collision.gameObject.CompareTag("Trap"))
        {
            if (P.canTakeDamage)
            {
                P.PlayerHasBeenHit();
            }
        }
    }
}
