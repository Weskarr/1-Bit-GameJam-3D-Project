using System.Collections;
using System.Collections.Generic;
using TheFirstPerson;
using UnityEngine;

public class AudioPoster : MonoBehaviour
{
    FPSController movementScript;

    public float footstepDuration;

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
                EchoManager.instance.StartEcho();
                lastFootstepTime = Time.time;
                footstepIsPlaying = true;
            } else {
                if (movementScript.moveSpeed > 1) {
                    if (Time.time - lastFootstepTime > footstepDuration / movementScript.moveSpeed * Time.deltaTime) {
                        footstepIsPlaying = false;
                    }
                }
            }
        }
    }
}
