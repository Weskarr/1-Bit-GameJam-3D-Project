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

    public AudioClip powerOutage;

    [SerializeField]
    private Breaker[] breakers;
    // 0 = Utility
    // 1 = Bedroom
    // 2 = Bathroom
    // 3 = Kitchen
    // 4 = Dining
    // 5 = Living
    // 6 = Hall

    AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void stopOutage() {
        audioSource.Stop();
    }

    IEnumerator SoundPlay() {
        audioSource.PlayOneShot(powerOutage);
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioSource.Play();
    }

    public void turnAllPowerOff()
    {
        powered = false;
        foreach (var breaker in breakers)
        {
            breaker.BreakerIsOut();

            SoundPlay();
        }
    }
}
