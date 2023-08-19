using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    [SerializeField]
    private bool[] fuses;
    // From Left To Right

    public bool powered = true;

    [Header("Wwise Events")]
    public AK.Wwise.Event powerOutage;

    [SerializeField]
    private Breaker[] breakers;
    // 0 = Utility
    // 1 = Bedroom
    // 2 = Bathroom
    // 3 = Kitchen
    // 4 = Dining
    // 5 = Living
    // 6 = Hall

    public void stopOutage() {
        powerOutage.Stop(gameObject);
    }

    public void turnAllPowerOff()
    {
        powered = false;
        foreach (var breaker in breakers)
        {
            breaker.BreakerIsOut();
            powerOutage.Post(gameObject);
        }
    }
}
