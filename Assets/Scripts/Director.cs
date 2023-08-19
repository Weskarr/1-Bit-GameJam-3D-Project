using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    private static Director director;

    public static Director instance {
        get {
            return director;
        }
    }

    void OnEnable() { director = this; }

    void OnDisable() { director = null; }

    // Control the gameplay, entitys and events

    public int toolSpawnCount;

    public MasterMind thing;

    EchoManager echoManager;
    public FPSController player;

    public GameObject toolsIcon;
    public TMPro.TextMeshProUGUI toolText;
    public TMPro.TextMeshProUGUI hourText;

    public Transform asleepPoint;
    public Transform awakePoint;
    public Transform firstAwakePoint;

    public GameObject fuseBoxPath;
    public GameObject fuseBoxDoor;
    public FuseBox fuseBox;

    public GameObject bedPath;

    public float startPauseTime;
    public float wakeupTime;
    public float lightsOffWait;

    public GameObject wakeupSoundLocation;
    [Header("Wwise Events")]
    public AK.Wwise.Event[] wakeupSounds;

    [Header("Wwise Events")]
    public AK.Wwise.Event bedOutSound;
    [Header("Wwise Events")]
    public AK.Wwise.Event bedInSound;


    public GameObject[] switches;

    public bool canSleep = false;

    [Header("Wwise Events")]
    public AK.Wwise.Event firstChaseSound;
    public GameObject firstChaseText;
    public float firstChaseTextTime;
    public bool firstTimeChased = true;

    public GameObject winUI;

    bool lightsOut = false;

    float wakeupStartTime = 0;

    int hour = 0;
    int toolsLeft;

    int switchInd;
    void Start()
    {
        toolsIcon.SetActive(false);
        //toolText.enabled = false;
        wakePoint = firstAwakePoint;
        echoManager = EchoManager.instance;
        StartCoroutine(startProcess());
        Events.onTriggerEnter += listenTrigger;
    }

    bool firstTime = true;
    Transform wakePoint;
    void Update() {
        if(wakeupStartTime > 0) {
            float p = (Time.time - wakeupStartTime) / wakeupTime;
            if (p >= 1) {
                wakeupStartTime = 0;
                player.transform.position = wakePoint.position;
                player.transform.rotation = wakePoint.rotation;
                if (!firstTime) player.movementEnabled = true;
                else wakePoint = awakePoint;
                player.mouseLookEnabled = true;
                GetComponent<ToolSpawn>().SpawnTools(toolSpawnCount);
                toolsLeft = toolSpawnCount;
            } else {
                Vector3 pos = Vector3.Lerp(asleepPoint.position, wakePoint.position, p);
                Quaternion rot = Quaternion.Lerp(asleepPoint.rotation, wakePoint.rotation, p);
                player.transform.position = pos;
                player.transform.rotation = rot;
            }
        }
        if (lightsOut && fuseBox.powered) {
            echoManager.LightsOn();
            lightsOut = false;
            fuseBoxPath.SetActive(false);
            fuseBox.stopOutage();
            canSleep = true;
            toolsIcon.SetActive(false);
            //toolText.enabled = false;
            bedPath.SetActive(true);
            // Despawn entity
            thing.EntityLeavesPlayField();
        }

    }

    public void GetTool(GameObject tool) {
        toolsLeft--;
        toolText.text = string.Format("{0}/{1}", toolSpawnCount - toolsLeft, toolSpawnCount);
        GetComponent<ToolSpawn>().RemoveTool(tool);
        if (toolsLeft <= 0) {
            fuseBoxPath.SetActive(true);
            fuseBoxDoor.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void GetFlashlight() {
        player.movementEnabled = true;
        echoManager.LightsOn();
    }

    void PlayRandomWakeupSound() {
        int i = Random.Range(0, wakeupSounds.Length);
        wakeupSounds[i].Post(wakeupSoundLocation);
    }

    IEnumerator startProcess() {
        player.transform.position = asleepPoint.position;
        player.transform.rotation = asleepPoint.rotation;
        player.movementEnabled = false;
        player.mouseLookEnabled = false;
        turningLightsOff = false;
        canSleep = false;
        echoManager.LightsOff();
        RandomizeSwitches();
        yield return new WaitForSeconds(startPauseTime);
        PlayRandomWakeupSound();
        yield return new WaitForSeconds(startPauseTime);
        if (!firstTime) {
            echoManager.LightsOn();
            hour++;
            hourText.text = string.Format("{0}", hour);
        }
        bedOutSound.Post(player.gameObject);
        wakeupStartTime = Time.time;
    }

    IEnumerator lightsOff() {
        yield return new WaitForSeconds(lightsOffWait);
        fuseBox.powered = false;
        fuseBox.turnAllPowerOff();
        lightsOut = true;
        echoManager.LightsOff();
        if (toolsLeft > 0) fuseBoxDoor.GetComponent<BoxCollider>().enabled = false; // In event player gets all tools pre lights out, leave collider on
        fuseBoxDoor.GetComponent<OpenDoor>().Close();

        toolsIcon.SetActive(true);
        //toolText.enabled = true;
        toolText.text = string.Format("{0}/{1}", toolSpawnCount - toolsLeft, toolSpawnCount);

        if(hour > 0) {
            thing.SpawnEntity();
        }
    }

    void RandomizeSwitches() {
        switchInd = Random.Range(0, switches.Length);
        for (int i = 0; i < switches.Length; i++) {
            if (i != switchInd) {
                switches[i].SetActive(false);
            } else {
                switches[i].SetActive(true);
            }
        }
    }

    IEnumerator win() {
        player.transform.position = asleepPoint.position;
        player.transform.rotation = asleepPoint.rotation;
        player.movementEnabled = false;
        player.mouseLookEnabled = false;
        turningLightsOff = false;
        canSleep = false;
        echoManager.LightsOff();
        winUI.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }

    public void sleep() {
        if (canSleep) {
            bedPath.SetActive(false);
            player.GetComponent<FPSController>().moving = false;
            firstTime = false;
            bedInSound.Post(player.gameObject);
            if(hour > 4) {
                StartCoroutine(win());
            } else {
                StartCoroutine(startProcess());
            }
        }
    }
    bool turningLightsOff = false;
    void listenTrigger(string name) {
        if (!turningLightsOff && name == "Bedroom Exit") {
            // Start lights off countdown
            turningLightsOff = true;
            StartCoroutine(lightsOff());
        }
    }

    IEnumerator HideChaseText() {
        yield return new WaitForSeconds(firstChaseTextTime);
        firstChaseText.SetActive(false);
    }

    public void FirstChase() {
        Debug.Log("Showing chase text");
        firstTimeChased = false;
        firstChaseSound.Post(player.gameObject);
        firstChaseText.SetActive(true);
        StartCoroutine(HideChaseText());
    }
}
