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

    public float startPauseTime;
    public float wakeupTime;
    public float lightsOffWait;


    public GameObject[] switches;

    public bool canSleep = false;

    float wakeupStartTime = 0;

    int switchInd;
    void Start()
    {
        echoManager = EchoManager.instance;
        StartCoroutine(startProcess());
        Events.onTriggerEnter += listenTrigger;
    }

    void Update() {
        if(wakeupStartTime > 0) {
            float p = (Time.time - wakeupStartTime) / wakeupTime;
            if (p >= 1) {
                wakeupStartTime = 0;
                player.transform.position = awakePoint.position;
                player.transform.rotation = awakePoint.rotation;
                player.movementEnabled = true;
                player.mouseLookEnabled = true;
                canSleep = false;
            } else {
                Vector3 pos = Vector3.Lerp(asleepPoint.position, awakePoint.position, p);
                Quaternion rot = Quaternion.Lerp(asleepPoint.rotation, awakePoint.rotation, p);
                player.transform.position = pos;
                player.transform.rotation = rot;
            }
        }
    }

    IEnumerator startProcess() {
        player.transform.position = asleepPoint.position;
        player.transform.rotation = asleepPoint.rotation;
        player.movementEnabled = false;
        player.mouseLookEnabled = false;
        echoManager.LightsOff();
        RandomizeSwitches();
        yield return new WaitForSeconds(startPauseTime);
        echoManager.LightsOn();
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
