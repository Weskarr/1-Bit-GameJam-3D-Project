using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void RetryGame()
    {
        SceneManager.LoadScene("GameplayScene");
        Debug.Log("Retry");
    }

}
