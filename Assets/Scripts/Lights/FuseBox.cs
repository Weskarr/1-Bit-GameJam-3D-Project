using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    [SerializeField]
    private bool[] fuses;
    // From Left To Right


    [SerializeField]
    private Breaker[] breakers;
    // 0 = Utility
    // 1 = Bedroom
    // 2 = Bathroom
    // 3 = Kitchen
    // 4 = Dining
    // 5 = Living
    // 6 = Hall

    private void turnAllPowerOff()
    {
        foreach (var breaker in breakers)
        {
            breaker.BreakerIsOut();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        turnAllPowerOff();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
