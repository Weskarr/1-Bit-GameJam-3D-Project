using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySound : MonoBehaviour
{
    public AudioClip[] footstepSounds;

    public AudioSource footstepSource;
    public AudioSource enterSource;
    public AudioSource idleSource;
    public AudioSource screamSource;
    public AudioSource chaseSource;

    public bool quiet = false;

    public void StopAll() {
        StopFootstep();
        StopEnter();
        StopIdle();
        StopScream();
        StopChase();
    }

    public void PlayFootstep() {
        if (quiet) return;
        int i = Random.Range(0, footstepSounds.Length);
        footstepSource.PlayOneShot(footstepSounds[i]);
    }

    public void StopFootstep() {
        footstepSource.Stop();
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
