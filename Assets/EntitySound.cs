using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySound : MonoBehaviour
{
    [Header("Wwise Events")]
    public AK.Wwise.Event footstepSound;
    [Header("Wwise Events")]
    public AK.Wwise.Event glowSound;
    [Header("Wwise Events")]
    public AK.Wwise.Event enterSound;
    [Header("Wwise Events")]
    public AK.Wwise.Event idleSound;
    [Header("Wwise Events")]
    public AK.Wwise.Event screamSound;
    [Header("Wwise Events")]
    public AK.Wwise.Event chaseSound;

    public bool quiet = false;

    public void StopAll() {
        StopFootstep();
        StopGlow();
        StopEnter();
        StopIdle();
        StopScream();
        StopChase();
    }

    public void PlayFootstep() {
        if (quiet) return;
        footstepSound.Post(gameObject);
    }

    public void StopFootstep() {
        footstepSound.Stop(gameObject);
    }

    public void PlayGlow() {
        glowSound.Post(gameObject);
    }

    public void StopGlow() {
        glowSound.Stop(gameObject);
    }

    public void PlayEnter() {
        enterSound.Post(gameObject);
    }

    public void StopEnter() {
        enterSound.Stop(gameObject);
    }

    public void PlayIdle() {
        if (quiet) return;
        idleSound.Post(gameObject);
    }

    public void StopIdle() {
        idleSound.Stop(gameObject);
    }

    public void PlayScream() {
        screamSound.Post(gameObject);
    }

    public void StopScream() {
        screamSound.Stop(gameObject);
    }

    public void PlayChase() {
        chaseSound.Post(gameObject);
    }

    public void StopChase() {
        chaseSound.Stop(gameObject);
    }



}
