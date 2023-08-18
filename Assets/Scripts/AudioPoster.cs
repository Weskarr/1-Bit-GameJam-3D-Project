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

    [Header("Wwise Events")]
    public AK.Wwise.Event myFootstep;

    void Start()
    {
        movementScript = GetComponent<FPSController>();
        lastFootstepTime = Time.time;
    }


    void Update()
    {
        if (movementScript.moving) {
            if (!footstepIsPlaying) {
                myFootstep.Post(gameObject);
                // StartCoroutine(delayedSound());
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

    IEnumerator delayedSound() {
        yield return new WaitForSeconds(behindStepDelay);
        myFootstep.Post(behindGO);
    }
}
