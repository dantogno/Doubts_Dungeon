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

    [Header("References")]
    Player playerRef;
    [SerializeField] GameObject HitBox;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        ListenForAttack();
    }


    // M I S C   M E T H O D S

    void ListenForAttack()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            UnityEngine.Debug.Log("MELEE ATTACK PERFORMED!!");
            Instantiate(slashFX, this.transform);
            Attacked = true;
        }
    }
    public void Performed()
    {
        Attacked = false;   
        Destroy(slashFX);
    }
}
