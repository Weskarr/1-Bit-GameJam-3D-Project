using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private float animationTime = 2f;

    [SerializeField]
    private bool startGame = false;

    [SerializeField]
    private Animator animator;

    
    public void LoadGame()
    {
        if (startGame == false)
        {
            startGame = true;
            StartCoroutine(StartAnimation());
        }
    }

    IEnumerator StartAnimation()
    {
        animator.SetBool("CrossAnimation", true);
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene("GameplayScene");
    }
}
