using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Wwise Events")]
    public AK.Wwise.Event OnButtonClick;

    public void clickSound()
    {
        OnButtonClick.Post(gameObject);
    }

    public void LoadGame()
    {
        clickSound();
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("GameplayScene", LoadSceneMode.Additive);
    }
}
