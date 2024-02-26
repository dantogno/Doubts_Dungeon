using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalState : MonoBehaviour
{
    [SerializeField]
    public GameObject PortalRef;
    [SerializeField]
    public Collider PortalRefCollider;
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

    [SerializeField] private bool isWaveEnemyRoom;

    [SerializeField] private bool isSurvivalEnemyRoom;

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
                EnemyManagerRef = FindObjectOfType<EnemyManager>();
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
