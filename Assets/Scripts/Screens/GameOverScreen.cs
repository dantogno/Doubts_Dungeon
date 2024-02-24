using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Needed!
public class GameOverScreen : MonoBehaviour
{
    public GameObject GameOverObject;
    // Start is called before the first frame update

    private void OnEnable()
    {
        GameManager.OnPlayerDeath += SetUp;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerDeath -= SetUp;
    }

    public void SetUp()
    {
       GameOverObject.SetActive(true);

    }

    public void RestartButton()
    {
        SceneManager.LoadScene("HubRoom",LoadSceneMode.Single);
        GameOverObject.SetActive(false);

    }


}
