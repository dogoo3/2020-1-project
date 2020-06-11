using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLocalSound()
    {
        audioSource.Stop();
        audioSource.Play();
    }
}
