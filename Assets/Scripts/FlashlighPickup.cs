using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlighPickup : MonoBehaviour, Interactable
{
    AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator SoundPlay() {
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        Destroy(gameObject);
    }

    public void Interact() {
        Director.instance.GetFlashlight();
        StartCoroutine(SoundPlay());
        
    }


}
