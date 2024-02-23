using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalState : MonoBehaviour
{
    [SerializeField]
    private GameObject PortalRef;
    [SerializeField]
    private Collider PortalRefCollider;
    [SerializeField]
    private bool Closed;
    [SerializeField]
    private bool Active;
    [SerializeField]
    private bool IsEntryPortal;
    [SerializeField]
    private bool isCombatRoom;
    [SerializeField]
    private EnemyManager EnemyManagerRef;

    [SerializeField]
    private TransitionManager TransitionManagerRef;

    // Start is called before the first frame update
    void Start()
    {
        TransitionManagerRef = FindObjectOfType<TransitionManager>();

        if (isCombatRoom == true)
        {
            if (EnemyManagerRef == null)
            {
                EnemyManagerRef.GetComponent<EnemyManager>();
            }

            if (IsEntryPortal == true)
            {
                PortalRef.SetActive(true);
                PortalRefCollider.enabled = false;
            }
            else if (IsEntryPortal == false)
            {
                PortalRef.SetActive(false);
                PortalRefCollider.enabled = false;
            }
        }
        else if (isCombatRoom == false)
        {
            if (IsEntryPortal == false)
            {
                PortalRef.SetActive(true);
                PortalRefCollider.enabled = true;
            }
            else if (IsEntryPortal == true)
            {
                PortalRef.SetActive(true);
                PortalRefCollider.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCombatRoom == true)
        {
            if (EnemyManagerRef.CurrentWave < EnemyManagerRef.NumOfWaves)
            {
                Closed = true;
                Active = false;
            }
            else if (EnemyManagerRef.CurrentWave >= EnemyManagerRef.NumOfWaves)
            {
                Closed = false;
                Active = true;
            }
        }
        else if(isCombatRoom == false)
        {
            Closed = false;
            Active = true;
        }

        CheckForRoomChange();
    }

    public void CheckForRoomChange()
    {
        if (Closed == true && Active == false)
        {
            return;
        }
        else if (Closed == false && Active == true)
        {
            ActivatePortal();
        }
    }

    public void ActivatePortal()
    {
        if (Closed == false && Active == true)
        {
            PortalRef.SetActive(true);
            PortalRefCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsEntryPortal == false)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                TransitionManagerRef.WhichRoom();
                TransitionManagerRef.LoadScene();
            }
        }
    }
}
