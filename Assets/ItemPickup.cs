using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, Interactable {
    [Header("Wwise Events")]
    public AK.Wwise.Event takeSound;
    public void Interact() {
        Director.instance.GetTool(gameObject);
        takeSound.Post(gameObject);
        Destroy(gameObject);
    }
}

