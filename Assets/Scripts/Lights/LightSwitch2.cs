using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch2 : MonoBehaviour, Interactable
{
    public GameObject[] lights;

    [SerializeField]
    private GameObject lightSwitchButton;

    private bool isLightOn = true;

    private void Start()
    {
        if (isLightOn)
        {
            turnOnOrOff();
        }
    }

    public void Interact()
    {
        turnOnOrOff();
    }

    private void turnOnOrOff ()
    {
        if (isLightOn == true)
        {
            //turn off
            foreach (GameObject light in lights)
            {
                light.SetActive(false);
            }
            isLightOn = false;
            lightSwitchButton.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            //turn on
            foreach (GameObject light in lights)
            {
                light.SetActive(true);
            }
            isLightOn = true;
            lightSwitchButton.transform.localRotation = Quaternion.Euler(-12f, 0f, 0f);
        }

    }
}
