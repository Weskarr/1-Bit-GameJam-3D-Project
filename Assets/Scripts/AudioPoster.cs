using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
using UnityEngine;

public class AudioPoster : MonoBehaviour
{
    FPSController movementScript;

    public GameObject behindGO;

    public float footstepDuration;
    public float behindStepDelay;

    //Wwise
    private bool footstepIsPlaying = false;
    private float lastFootstepTime = 0;

    public AudioClip[] footsteps;

    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        movementScript = GetComponent<FPSController>();
        lastFootstepTime = Time.time;
    }


    void Update()
    {
        if (movementScript.moving) {
            if (!footstepIsPlaying) {
                int i = Random.Range(0, footsteps.Length);
                audioSource.PlayOneShot(footsteps[i]);
                EchoManager.instance.StartEcho();
                lastFootstepTime = Time.time;
                footstepIsPlaying = true;
            } else {
                if (movementScript.moving) {
                    if (Time.time > lastFootstepTime + footstepDuration) {
                        footstepIsPlaying = false;
                    }
                }
            }
        }
    }

}
