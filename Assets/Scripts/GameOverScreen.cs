using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject GameOverObject;
    // Start is called before the first frame update

    private void OnEnable()
    {
        PlayerMovement.OnPlayerDeath += SetUp;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= SetUp;
    }

    public void SetUp()
    {
       GameOverObject.SetActive(true);

    }

    public void RestartButton()
    {
        SceneManager.LoadScene("HubRoom",LoadSceneMode.Single);
    }
}
