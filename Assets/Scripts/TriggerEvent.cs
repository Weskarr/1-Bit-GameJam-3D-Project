using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{

    public string triggerName;


    void OnTriggerEnter(Collider Other) {
        if (Other.gameObject.name == "Player") {
            Events.CallTriggerEnter(triggerName);
        }
    }

}
