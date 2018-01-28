using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip plugIn;
    public AudioClip plugOut;
    public AudioClip connected;
    public AudioClip error;

    public void PlayPlugIn()
    {
        audioSource.clip = plugIn;
        audioSource.Play();
    }

    public void PlayPlugOut()
    {
        audioSource.clip = plugOut;
        audioSource.Play();
    }

    public void PlayConnected()
    {
        audioSource.clip = connected;
        audioSource.Play();
    }

    public void PlayError()
    {
        audioSource.clip = error;
        audioSource.Play();
    }
}
