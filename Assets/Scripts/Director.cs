using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
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

    public AudioSource door1Sound;

    public float startPauseTime;
    public float lightsOffWait;


    public GameObject[] switches;

    public bool canSleep = false;

    int switchInd;
    void Start()
    {
        echoManager = EchoManager.instance;
        StartCoroutine(startProcess());
    }

    IEnumerator startProcess() {
        player.movementEnabled = false;
        echoManager.LightsOff();
        RandomizeSwitches();
        yield return new WaitForSeconds(startPauseTime);
        echoManager.LightsOn();
        player.movementEnabled = true;
        door1Sound.Play();
        canSleep = false;
        StartCoroutine(lightsOff());
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

    void Update()
    {
        
    }
}
