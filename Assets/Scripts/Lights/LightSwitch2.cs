using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch2 : MonoBehaviour, Interactable
{
    [SerializeField]
    private GameObject[] lights;
    [SerializeField]
    private MeshRenderer[] interactionHighLighted;

    [SerializeField]
    private GameObject lightSwitchButton;
    [SerializeField]
    private Material interactionHighLightMaterial;
    [SerializeField]
    private Material normalMaterial;


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
            foreach (MeshRenderer ren in interactionHighLighted)
            {
                ren.material = normalMaterial;
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
            foreach (MeshRenderer ren in interactionHighLighted)
            {
                ren.material = interactionHighLightMaterial;
            }
            isLightOn = true;
            lightSwitchButton.transform.localRotation = Quaternion.Euler(-12f, 0f, 0f);
        }

    }
}
