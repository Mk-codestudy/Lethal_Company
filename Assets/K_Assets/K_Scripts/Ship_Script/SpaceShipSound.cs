using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipSound : MonoBehaviour
{
    public AudioClip[] audioClips;
    AudioSource audioSource;

    bool introduce;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!introduce)
        {
            Invoke("Toemployee", 3.0f);
            introduce = true;
        }
    }

    void Toemployee()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

}
