using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField]
    PlayerMovement playerMovement;

    [SerializeField]
    GameObject Menty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Stabilizer, Adrenalin, Histamine, Acetylcholine, Endorphins, Noradrenaline, Dopamine
    public void UseBuff(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Stabilizer:
                //Increases Stablizer count

                break;
            case ItemType.Adrenalin:
                //Mario Super star (invincible for a durration)

                break;
            case ItemType.Histamine:
                //Lower enemy health in a room | One Time

                break;
            case ItemType.Acetylcholine:
                //Increased Damage | Permanent

                break;
            case ItemType.Endorphins:
                //Shield | Duration

                break;
            case ItemType.Noradrenaline:
                //Increased Speed | Duration

                break;
            case ItemType.Dopamine:
                //Increases Currency Drop | Perminent

                break;
        }
    }

    public void ResetBuffs()
    {

    }
}
