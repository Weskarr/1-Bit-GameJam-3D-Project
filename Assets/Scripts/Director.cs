using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
using Unity.VisualScripting;
using UnityEngine;

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

    EchoManager echoManager;
    public FPSController player;

    public Transform asleepPoint;
    public Transform awakePoint;
    public Transform firstAwakePoint;

    public float startPauseTime;
    public float wakeupTime;
    public float lightsOffWait;

    [Header("Wwise Events")]
    public AK.Wwise.Event wakeupSound;


    public GameObject[] switches;

    public bool canSleep = false;

    float wakeupStartTime = 0;

    int switchInd;
    void Start()
    {
        wakePoint = firstAwakePoint;
        echoManager = EchoManager.instance;
        StartCoroutine(startProcess());
        Events.onTriggerEnter += listenTrigger;
    }

    bool firstTIme = true;
    Transform wakePoint;
    void Update() {
        if(wakeupStartTime > 0) {
            float p = (Time.time - wakeupStartTime) / wakeupTime;
            if (p >= 1) {
                wakeupStartTime = 0;
                player.transform.position = wakePoint.position;
                player.transform.rotation = wakePoint.rotation;
                if (!firstTIme) player.movementEnabled = true;
                else wakePoint = awakePoint;
                player.mouseLookEnabled = true;
                canSleep = false;
            } else {
                Vector3 pos = Vector3.Lerp(asleepPoint.position, wakePoint.position, p);
                Quaternion rot = Quaternion.Lerp(asleepPoint.rotation, wakePoint.rotation, p);
                player.transform.position = pos;
                player.transform.rotation = rot;
            }
        }
    }

    public void GetFlashlight() {
        player.movementEnabled = true;
        echoManager.LightsOn();
    }

    IEnumerator startProcess() {
        player.transform.position = asleepPoint.position;
        player.transform.rotation = asleepPoint.rotation;
        player.movementEnabled = false;
        player.mouseLookEnabled = false;
        echoManager.LightsOff();
        RandomizeSwitches();
        yield return new WaitForSeconds(startPauseTime);
        wakeupSound.Post(player.gameObject);
        yield return new WaitForSeconds(startPauseTime);
        wakeupStartTime = Time.time;
        GetComponent<ToolSpawn>().SpawnTools(3);
    }

    IEnumerator lightsOff() {
        yield return new WaitForSeconds(lightsOffWait);
        echoManager.LightsOff();
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

    public void sleep() {
        if (canSleep) {
            StartCoroutine(startProcess());
        }
    }

    void listenTrigger(string name) {
        if (name == "Bedroom Exit") {
            // Start lights off countdown
            StartCoroutine(lightsOff());
        }
    }
}
