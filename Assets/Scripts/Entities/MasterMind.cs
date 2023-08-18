using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterMind : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float enteringWindowTime = 30f;
    [SerializeField]
    private float openingWindowTime = 30f;
    [SerializeField]
    private float spawnEntityTime = 10f;
    [SerializeField]
    private bool entitiesAllowed = true;
    [SerializeField]
    private bool entitiesVanishOnAllLight = true;
    [SerializeField]
    private bool entitiesKillsFuseBoxOnSpawn = true;

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
    private FuseBox fusebox;
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
    [SerializeField]
    private bool spawningEntity = false;

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

    [Header("Spawning")]
    [SerializeField]
    private GameObject bedroom_BuiltIn;
    [SerializeField]
    private GameObject utility_Cupboard;
    [SerializeField]
    private GameObject bedroom_Closet;
    [SerializeField]
    private GameObject bathroom_Shower;
    [SerializeField]
    private GameObject currentSpawner;
    [SerializeField]
    private GameObject currentEntity;

    private void Start()
    {
        AssignEntitiesFromChildren();
    }
    private void Update()
    {
        if (entitiesAllowed)
        {
            if (!AnyoneOnPlayField)
            {
                if (!spawningEntity)
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
            }
            else
            {
                if (enteringWindow)
                {
                    StopCoroutine(EnteringWindowCoroutine());
                    enteringWindow = false;
                }
                if (openingWindows)
                {
                    StopCoroutine(OpeningWindowsCoroutine());
                    openingWindows = false;
                }

                if (entitiesVanishOnAllLight)
                {
                    CheckIfLightIsOff();
                    if (aLightIsOff == false && AnyoneOnPlayField == true)
                    {
                        EntityLeavesPlayField();
                    }
                }
            }
        }
    }

    // Control
    public void AllowEntitiesToPlay()
    {
        entitiesAllowed = true;
    }
    public void NoEntitiesAllowedToPlay()
    {
        entitiesAllowed = false;
        StopAllCoroutines();

        if (currentEntity != null)
            EntityLeavesPlayField();
    }
    //

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
        CheckIfLightIsOff();
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
        CheckIfLightIsOff();
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
    private void FuseboxOut()
    {
        if (entitiesKillsFuseBoxOnSpawn)
        {
            fusebox.turnAllPowerOff();
            CheckIfLightIsOff();
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
    private void RandomEntity()
    {
        int random = Random.Range(0, 5);

        switch (random)
        {
            case 0:
                childAtPlay = true;
                currentEntity = child;
                break;
            case 1:
                drunkAtPlay = true;
                currentEntity = drunk;
                break;
            case 2:
                stalkerAtPlay = true;
                currentEntity = stalker;
                break;
            case 3:
                madameAtPlay = true;
                currentEntity = madame;
                break;
            case 4:
                grannyAtPlay = true;
                currentEntity = granny;
                break;
        }
    }
    private void GiveMeARandomSpawn()
    {
        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:
                currentSpawner = bedroom_BuiltIn;
                break;
            case 1:
                currentSpawner = utility_Cupboard;
                break;
            case 2:
                currentSpawner = bedroom_Closet;
                break;
            case 3:
                currentSpawner = bathroom_Shower;
                break;
        }
    }
    public void EntityLeavesPlayField()
    {
        if (childAtPlay == true)
        {
            child.SetActive(false);
            childAtPlay = false;
        }
        else if (drunkAtPlay == true)
        {
            drunk.SetActive(false);
            drunkAtPlay = false;
        }
        else if (stalkerAtPlay == true)
        {
            stalker.SetActive(false);
            stalkerAtPlay = false;
        }
        else if (madameAtPlay == true)
        {
            madame.SetActive(false);
            madameAtPlay = false;
        }
        else if (grannyAtPlay == true)
        {
            granny.SetActive(false);
            grannyAtPlay = false;
        }
        currentEntity = null;
        AnyEntitiesOnThePlayField();
    }
    public void SpawnEntity()
    {
        StopAllCoroutines();
        AnyEntitiesOnThePlayField();
        if (currentEntity == null)
        {
            RandomEntity();
            StartCoroutine(SpawningEntityCoroutine());
        }
    }
    //



    IEnumerator SpawningEntityCoroutine()
    {
        spawningEntity = true;
        GiveMeARandomSpawn();
        currentSpawner.transform.GetComponent<Spawner>().ParticlesOn();

        yield return new WaitForSeconds(spawnEntityTime);
        FuseboxOut();
        yield return new WaitForSeconds(1f);
        currentEntity.transform.position = currentSpawner.transform.position;
        currentEntity.SetActive(true);
        currentSpawner.transform.GetComponent<Spawner>().ParticlesOff();
        AnyEntitiesOnThePlayField();
        spawningEntity = false;
    }

    IEnumerator OpeningWindowsCoroutine()
    {
        openingWindows = true;
        yield return new WaitForSeconds(openingWindowTime);
        OpenRandomWindow();
        openingWindows = false;
    }

    IEnumerator EnteringWindowCoroutine()
    {
        enteringWindow = true;
        yield return new WaitForSeconds(enteringWindowTime);
        SpawnEntity();
        enteringWindow = false;
    }
}
