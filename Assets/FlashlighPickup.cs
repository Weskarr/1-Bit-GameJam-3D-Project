using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlighPickup : MonoBehaviour, Interactable
{
    [Header("Wwise Events")]
    public AK.Wwise.Event takeSound;
    public void Interact() {
        Director.instance.GetFlashlight();
        takeSound.Post(gameObject);
        Destroy(gameObject);
    }


}
