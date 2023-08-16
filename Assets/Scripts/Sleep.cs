using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour, Interactable
{
    public void Interact() {
        Director.instance.sleep();
    }
}
