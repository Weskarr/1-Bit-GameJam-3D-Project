using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject hands;
    public void DisableHands() {
        flashlight.SetActive(false);
        hands.SetActive(false);
    }

    public void EnableHands() {
        flashlight.SetActive(true);
        hands.SetActive(true);
    }
}
