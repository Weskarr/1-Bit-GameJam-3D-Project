using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlighPickup : MonoBehaviour, Interactable
{
    AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact() {
        Director.instance.GetFlashlight();
        audioSource.Play();
        Destroy(gameObject);
    }


}
