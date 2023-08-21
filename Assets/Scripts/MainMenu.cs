using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public AudioSource menuMusic;

    float startVol;
    private void Start() {
        startVol = menuMusic.volume;
    }

    public void LoadGame()
    {
        if (startGame == false)
        {
            startGame = true;
            aniStart = Time.time;
            StartCoroutine(StartAnimation());
        }
    }

    float aniStart;
    private void Update() {
        if(startGame) menuMusic.volume = Mathf.Lerp(startVol, 0, (Time.time - aniStart) / (animationTime));
    }

    IEnumerator StartAnimation()
    {
        animator.SetBool("CrossAnimation", true);
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene("GameplayScene");
    }
}
