using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, Interactable {

    AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void Interact() {
        Director.instance.GetTool(gameObject);
        audioSource.Play();
        Destroy(gameObject);
    }
}

