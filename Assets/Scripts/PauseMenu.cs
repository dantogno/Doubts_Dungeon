using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseScreen;

    [SerializeField]
    private int PressAmount = 0;
    // Start is called before the first frame update

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                Pause();
        }
    }

     public void Pause()
    {
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
       
    }

    public void Continue()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
        PressAmount = 0;
    }

    public void Leave()
    {
        Application.Quit();
    }
}
