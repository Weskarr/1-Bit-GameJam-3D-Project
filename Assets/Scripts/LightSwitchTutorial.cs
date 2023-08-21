using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchTutorial : MonoBehaviour
{
    public TMPro.TextMeshPro tutorialText1;
    public TMPro.TextMeshPro tutorialText2;
    public GameObject doorTutorial;
    LightSwitch2 ls;
    void Start()
    {
        ls = GetComponent<LightSwitch2>();
    }

    bool done = false;
    void Update()
    {
        if (done) return;
        if (ls.isLightOn) {
            tutorialText1.enabled = false;
            tutorialText2.enabled = true;
            doorTutorial.GetComponent<BoxCollider>().enabled = true;
        }
        if (doorTutorial.GetComponent<OpenDoor>().moving) {
            tutorialText2.enabled = false;
            done = true;
        }
    }
}
