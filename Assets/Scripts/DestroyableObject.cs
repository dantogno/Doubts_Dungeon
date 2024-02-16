using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject IntitalState;

    [SerializeField]
    private GameObject BrokeState;

    [SerializeField]
    private float WaitTimeDespawn;

    [SerializeField]
    private InventoryManager InventoryManager;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            BreakObject();
        }
    }

    private void BreakObject()
    {
        IntitalState.SetActive(false);
        BrokeState.SetActive(true);
        InventoryManager.player.Currancy++;
        StartCoroutine(TimeToDestroy());
    }

    private IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(WaitTimeDespawn);
        Destroy(gameObject);
    }
}
