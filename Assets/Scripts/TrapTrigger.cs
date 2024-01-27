using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField]
    private Trap TrapRef;



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TrapRef.TrapAni.SetTrigger("SpikeUp");


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TrapRef.TrapAni.ResetTrigger("SpikeUp");

        }
    }
}
