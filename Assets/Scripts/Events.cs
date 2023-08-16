using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{

    private static Events ev;

    public static Events instance {
        get {
            return ev;
        }
    }

    public static event System.Action<string> onTriggerEnter;
    public static void CallTriggerEnter(string name) { onTriggerEnter?.Invoke(name);  }

    public static event System.Action onLightsOff;
    public static void CallLightsOff() { onLightsOff?.Invoke(); }

    public static event System.Action onLightsOn;
    public static void CallLightsOn() { onLightsOn?.Invoke(); }

}
