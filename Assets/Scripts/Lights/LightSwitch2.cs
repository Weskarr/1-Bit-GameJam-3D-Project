using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch2 : MonoBehaviour, Interactable
{
    [SerializeField]
    private MasterMind myMasterMind;
    [SerializeField]
    private int lightIdentity;


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
    private bool hasPower = true;

    private void Start()
    {
        turnOn();
    }

    public void Interact()
    {
        switchLight();
    }

    private void switchLight()
    {

        if (isLightOn == true)
        {
            turnOff();
        }
        else
        {
            turnOn();
        }
    }

    public void powerIsBack()
    {
        hasPower = true;
        if (isLightOn == true)
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
            myMasterMind.LightIsOn(lightIdentity);
        }
    }
    public void powerIsGone()
    {
        hasPower = false;
        
        //turn off
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
        foreach (MeshRenderer ren in interactionHighLighted)
        {
            ren.material = normalMaterial;
        }

        myMasterMind.LightIsOff(lightIdentity);
    }

    private void turnOff()
    {
        if (hasPower)
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

            myMasterMind.LightIsOff(lightIdentity);
        }

        isLightOn = false;
        lightSwitchButton.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void turnOn ()
    {
        if (hasPower)
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

            myMasterMind.LightIsOn(lightIdentity);
        }

        isLightOn = true;
        lightSwitchButton.transform.localRotation = Quaternion.Euler(-12f, 0f, 0f);
    }
}
