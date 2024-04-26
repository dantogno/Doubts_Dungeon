using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    // P R O P E R T I E S
    private int damage;
    private float coolDownDuration;
    public bool Attacked;
    public GameObject slashFX;
    public float MeleeRange;

    [Header("References")]
    Player playerRef;
    [SerializeField] GameObject HitBox;

    [Header("Animation Settings")]
    [SerializeField] Animator MeleeAnim;

    // U N I T Y   M E T H O D S
    private void OnLevelWasLoaded(int level)
    {
        GameObject go = GameObject.Find("HitBox");
        playerRef = go.GetComponent<Player>();
    }
    private void Awake()
    {
        playerRef = GetComponent<Player>();

        if (playerRef != null )
        {
            damage = playerRef.Damage;
            coolDownDuration = playerRef.damageCooldownDuration;
        }

        Attacked = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        MeleeAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ListenForAttack();
    }


    // M I S C   M E T H O D S

    void ListenForAttack()
    {
        //Input.GetKeyDown(KeyCode.RightShift)
        if (Input.GetButtonDown("Fire2"))
        {
            UnityEngine.Debug.Log("MELEE ATTACK PERFORMED!!");
            Attacked = true;
            //MeleeAnim.SetBool("Attack", true);
            MeleeAnim.Play("MeleeAttack");
            SpawnSlashFX();
            
        }
    }

    
    public void Performed()
    {
        Attacked = false;
        //MeleeAnim.SetBool("Attack", false);
        DestroyImmediate(slashFX);
    }
    void SpawnSlashFX()
    {
        

        RaycastHit hit;
        Ray dodgeRay = new Ray(transform.position, transform.forward);
        // if we hit something, play spawn the slashFX at the hit thing's position (play the VFX on that target)
        if (Physics.Raycast(dodgeRay, out hit, MeleeRange))
        {
            float distance = hit.distance;
            Debug.Log("HIT THING!!");

            if (hit.collider != null)
            {
                Debug.Log("PLAYED VFX ON THING!!");
                Instantiate(slashFX, hit.transform.position, Quaternion.identity, this.transform);
            }
            
        }
        // if we don't hit something, play the slashFX at the player's range
        else
        {
            Debug.Log("didn't hit thing. No VFX on target");
            Instantiate(slashFX, this.transform);
        }
    }
}
