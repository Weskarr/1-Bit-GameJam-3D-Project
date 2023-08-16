using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, Interactable {

    float startDoorRotation;
    public float endDoorRotation;
    public float doorRotationDuration;

    float startTime;
    bool opening = false;
    bool opened = false;

    [Header("Wwise Events")]
    public AK.Wwise.Event fuseOpen;

    private void Start() {
        startDoorRotation = transform.eulerAngles.y;
    }

    public void Interact() {
        
        //if (!opening && !opened) 
        if (!opening && !opened)
        {
            fuseOpen.Post(gameObject);
            opening = true;
            startTime = Time.time;
        }
        else if (!opening && opened)
        {
            opening = true;
            startTime = Time.time;
        }
    }

    private void Update() {

        if (opening)
        {
            if (!opened)
            {
                float p = (Time.time - startTime) / doorRotationDuration;
                if (p >= 1)
                {
                    transform.rotation = Quaternion.Euler(0, endDoorRotation, 0);
                    opened = true;
                    opening = false;
                }
                else
                {
                    float r = Mathf.LerpAngle(startDoorRotation, endDoorRotation, p);
                    transform.rotation = Quaternion.Euler(0, r, 0);
                }

            }
            else if (opened)
            {
                float p = (Time.time - startTime) / (doorRotationDuration / 2);
                if (p >= 1)
                {
                    transform.rotation = Quaternion.Euler(0, startDoorRotation, 0);
                    opened = false;
                    opening = false;
                }
                else
                {
                    float r = Mathf.LerpAngle(endDoorRotation, startDoorRotation, p);
                    transform.rotation = Quaternion.Euler(0, r, 0);
                }
            }

        }

    }

}
