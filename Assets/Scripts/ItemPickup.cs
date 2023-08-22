using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, Interactable {

    AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator SoundPlay() {
        audioSource.Play();
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitWhile(() => audioSource.isPlaying);
        Destroy(gameObject);
    }
    public void Interact() {
        Director.instance.GetTool(gameObject);
        StartCoroutine(SoundPlay());
    }
}

