using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnScreenConsole : MonoBehaviour
{
    // P R O P E R T I E S
    [SerializeField] TMP_Text[] Consoles;
    [SerializeField] bool IsOn;
    //[SerializeField] int NextOpenSlotIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        ResetConsoles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Log(string label, object content, int slotToUse)
    {
        if (IsOn)
        {
            string message = $"{label}: {content.ToString()}";
            Consoles[slotToUse].text = message;
        }
    }

    void ResetConsoles()
    {
        foreach (TMP_Text console in Consoles)
        {
            console.text = string.Empty;
        }
    }
}
