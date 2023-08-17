using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterMind : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField]
    private bool aWindowIsOpen = false;
    [SerializeField]
    private bool utilityWindowOpen = false; // ID 0
    [SerializeField]
    private bool livingOneWindowOpen = false; // ID 1
    [SerializeField]
    private bool livingTwoWindowOpen = false; // ID 2
    [SerializeField]
    private bool livingThreeWindowOpen = false; // ID 3
    [SerializeField]
    private bool livingFourWindowOpen = false; // ID 4
    [SerializeField]
    private bool bedroomWindowOpen = false; // ID 5
    [SerializeField]
    private bool bathroomWindowOpen = false; // ID 6
    [SerializeField]
    private Window[] windowsToOpen;


    [Header("Lights")]
    [SerializeField]
    private bool aLightIsOff = false;
    [SerializeField]
    private bool utilityLightIsOn; // ID 0
    [SerializeField]
    private bool bedroomLightIsOn; // ID 1
    [SerializeField]
    private bool bathroomLightIsOn; // ID 2
    [SerializeField]
    private bool kitchenLightIsOn; // ID  3
    [SerializeField]
    private bool diningLightIsOn; // ID 4
    [SerializeField]
    private bool livingLightIsOn; // ID 5
    [SerializeField]
    private bool hallwayLightIsOn; // ID 6

    [Header("Behaviour")]
    [SerializeField]
    private bool enteringWindow = false;
    [SerializeField]
    private bool openingWindows = false;

    [Header("Entities")]
    [SerializeField]
    private bool AnyoneOnPlayField;
    [SerializeField]
    private bool childAtPlay;
    [SerializeField]
    private bool drunkAtPlay;
    [SerializeField]
    private bool stalkerAtPlay;
    [SerializeField]
    private bool madameAtPlay;
    [SerializeField]
    private bool grannyAtPlay;
    private GameObject child;
    private GameObject drunk;
    private GameObject stalker;
    private GameObject madame;
    private GameObject granny;

    private void Start()
    {
        AssignEntitiesFromChildren();
    }
    private void Update()
    {
        if (!AnyoneOnPlayField)
        {
            CheckIfWindowIsOpen();

            if (aWindowIsOpen == true && enteringWindow == false)
            {
                StartCoroutine(EnteringWindowCoroutine());
            }
            else if (aWindowIsOpen == false && enteringWindow == true)
            {
                StopCoroutine(EnteringWindowCoroutine());
                enteringWindow = false;
            }
            
            
            if (aWindowIsOpen == false && openingWindows == false)
            {
                StartCoroutine(OpeningWindowsCoroutine());
            }
            else if (aWindowIsOpen && openingWindows == true)
            {
                StopCoroutine(OpeningWindowsCoroutine());
                openingWindows = false;
            }
        }
        else
        {
            CheckIfLightIsOff();
        }
    }



    // Windows
    public void WindowIsOpen(int ID)
    {
        switch (ID)
        {
            case 0:
                utilityWindowOpen = true;
                break;
            case 1:
                livingOneWindowOpen = true;
                break;
            case 2:
                livingTwoWindowOpen = true;
                break;
            case 3:
                livingThreeWindowOpen = true;
                break;
            case 4:
                livingFourWindowOpen = true;
                break;
            case 5:
                bedroomWindowOpen = true;
                break;
            case 6:
                bathroomWindowOpen = true;
                break;
        }
    }
    public void WindowIsClosed(int ID)
    {
        switch (ID)
        {
            case 0:
                utilityWindowOpen = false;
                break;
            case 1:
                livingOneWindowOpen = false;
                break;
            case 2:
                livingTwoWindowOpen = false;
                break;
            case 3:
                livingThreeWindowOpen = false;
                break;
            case 4:
                livingFourWindowOpen = false;
                break;
            case 5:
                bedroomWindowOpen = false;
                break;
            case 6:
                bathroomWindowOpen = false;
                break;
        }
    }
    private void CheckIfWindowIsOpen()
    {
        if (utilityWindowOpen == true ||
            livingOneWindowOpen == true ||
            livingTwoWindowOpen == true ||
            livingThreeWindowOpen == true ||
            livingFourWindowOpen == true ||
            bedroomWindowOpen == true ||
            bathroomWindowOpen == true)
        {
            aWindowIsOpen = true;
        }
        else
        {
            aWindowIsOpen = false;
        }
    }
    private void OpenRandomWindow()
    {
        int random = Random.Range(0, windowsToOpen.Count());

        if (windowsToOpen[random].close)
            windowsToOpen[random].Interact();
    }
    //

    // Lights
    public void LightIsOn(int ID)
    {
        switch (ID)
        {
            case 0:
                utilityLightIsOn = true;
                break;
            case 1:
                bedroomLightIsOn = true;
                break;
            case 2:
                bathroomLightIsOn = true;
                break;
            case 3:
                kitchenLightIsOn = true;
                break;
            case 4:
                diningLightIsOn = true;
                break;
            case 5:
                livingLightIsOn = true;
                break;
            case 6:
                hallwayLightIsOn = true;
                break;
        }
    }
    public void LightIsOff(int ID)
    {
        switch (ID)
        {
            case 0:
                utilityLightIsOn = false;
                break;
            case 1:
                bedroomLightIsOn = false;
                break;
            case 2:
                bathroomLightIsOn = false;
                break;
            case 3:
                kitchenLightIsOn = false;
                break;
            case 4:
                diningLightIsOn = false;
                break;
            case 5:
                livingLightIsOn = false;
                break;
            case 6:
                hallwayLightIsOn = false;
                break;
        }
    }
    private void CheckIfLightIsOff()
    {
        if (utilityLightIsOn == false ||
            bedroomLightIsOn == false ||
            bathroomLightIsOn == false ||
            kitchenLightIsOn == false ||
            diningLightIsOn == false ||
            livingLightIsOn == false ||
            hallwayLightIsOn == false)
        {
            aLightIsOff = true;
        }
        else
        {
            aLightIsOff = false;
        }
    }
    //

    // Entities
    private void AssignEntitiesFromChildren()
    {
        child = this.transform.GetChild(0).gameObject;
        drunk = this.transform.GetChild(1).gameObject;
        stalker = this.transform.GetChild(2).gameObject;
        madame = this.transform.GetChild(3).gameObject;
        granny = this.transform.GetChild(4).gameObject;
    }
    private void AnyEntitiesOnThePlayField()
    {
        if (childAtPlay == true ||
            drunkAtPlay == true ||
            stalkerAtPlay == true ||
            madameAtPlay == true ||
            grannyAtPlay == true)
        {
            AnyoneOnPlayField = true;
        }
        else
        {
            AnyoneOnPlayField = false;
        }
    }
    private void EntityEntersPlayField()
    {
        int random = Random.Range(0, 5);

        switch (random)
        {
            case 0:
                childAtPlay = true;
                child.SetActive(true);
                break;
            case 1:
                drunkAtPlay = true;
                drunk.SetActive(true);
                break;
            case 2:
                stalkerAtPlay = true;
                stalker.SetActive(true);
                break;
            case 3:
                madameAtPlay = true;
                madame.SetActive(true);
                break;
            case 4:
                grannyAtPlay = true;
                granny.SetActive(true);
                break;
        }

        AnyEntitiesOnThePlayField();
    }
    //

    IEnumerator OpeningWindowsCoroutine()
    {
        openingWindows = true;
        yield return new WaitForSeconds(30f);
        OpenRandomWindow();
        openingWindows = false;
    }

    IEnumerator EnteringWindowCoroutine()
    {
        enteringWindow = true;
        yield return new WaitForSeconds(30f);
        EntityEntersPlayField();
        enteringWindow = false;
    }
}
