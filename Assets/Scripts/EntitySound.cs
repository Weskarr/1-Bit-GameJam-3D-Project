using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySound : MonoBehaviour
{
    AudioSource footstepSource;
    AudioSource glowSource;
    AudioSource enterSource;
    AudioSource idleSource;
    AudioSource screamSource;
    AudioSource chaseSource;

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
        footstepSource.Play();
    }

    public void StopFootstep() {
        footstepSource.Stop();
    }

    public void PlayGlow() {
        glowSource.Play();
    }

    public void StopGlow() {
        glowSource.Stop();
    }

    public void PlayEnter() {
        enterSource.Play();
    }

    public void StopEnter() {
        enterSource.Stop();
    }

    public void PlayIdle() {
        if (quiet) return;
        idleSource.Play();
    }

    public void StopIdle() {
        idleSource.Stop();
    }

    public void PlayScream() {
        screamSource.Play();
    }

    public void StopScream() {
        screamSource.Stop();
    }

    public void PlayChase() {
        chaseSource.Play();
    }

    public void StopChase() {
        chaseSource.Stop();
    }



}
