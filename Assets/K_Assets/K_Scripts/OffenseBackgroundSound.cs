using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenseBackgroundSound : MonoBehaviour
{
    public AudioClip[] audioClips;
    AudioSource audioSource;

    public GameObject offense;
    public GameObject factory;

    bool okmusicstart; // 함선 착륙 후 배경음악 시작

    bool ofplay;
    bool fcplay;

    float fadeoutstart = 3; //페이드아웃 시작시간
    float currenttime;
    public float fadeDuration = 4.0f; // 페이드아웃에 걸리는 시간 (초)
    bool coutover;

    bool notfirst;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play(); //첫 할당 음악 재생
        Invoke("WaitForLanding", 7.0f);
    }

    void Update()
    {
        if (!coutover)
        {
            if (currenttime < fadeoutstart)
            {
                currenttime += Time.deltaTime;
            }
            else
            {
                StartCoroutine(FadeOut(audioSource, fadeDuration));
                coutover = true;
            }
        }


        if (okmusicstart)
        {
            if (offense.activeSelf && !factory.activeSelf)
            {
                fcplay = false;
                if (!ofplay)
                {
                    if (notfirst)
                    {
                        audioSource.clip = audioClips[2];
                        audioSource.Play();
                    }
                    Invoke("InvokeOF", 1f);
                    ofplay = true;
                    notfirst = true;
                }
            }
            else if (factory.activeSelf && !offense.activeSelf)
            {
                ofplay = false;
                if (!fcplay)
                {
                    audioSource.clip = audioClips[2];
                    audioSource.Play();
                    Invoke("InvokeFC", 1f);
                    fcplay = true;
                }
            }
        }
    }

    void WaitForLanding()
    {
        okmusicstart = true;
    }

    void InvokeOF()
    {
        audioSource.clip = audioClips[0];
        audioSource.volume = 0.8f;
        audioSource.Play();
        audioSource.loop = true;
    }

    void InvokeFC()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
        audioSource.loop = true;
    }

    IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = 1.0f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // 볼륨을 초기 상태로 되돌립니다.
    }


}
