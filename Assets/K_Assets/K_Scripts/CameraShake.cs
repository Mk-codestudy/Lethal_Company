using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //Camera cam;
    //Vector3 camOriginPos;

    public GameObject player;

    Vector3 originPos;
    public bool letsShake;

    void Start()
    {
        //cam = Camera.main;
        //camOriginPos = cam.transform.position;

        //originPos = gameObject.transform.position;
    }

    void Update()
    {
        if (letsShake)
        {
            StartCoroutine(CamShake(0.6f, 0.1f));

        }
    }

    IEnumerator CamShake(float duration, float magnitude) //카메라 흔들리게 하는 코루틴, float 작동시간, float 작동범위
    {
        float timer = 0;
        originPos = gameObject.transform.position;
        player.transform.SetParent(gameObject.transform);

        while (timer <= duration)
        {
            gameObject.transform.position = Random.insideUnitSphere * magnitude + originPos;
            timer += Time.deltaTime; //시간 흐르기
            yield return null;
        }

        gameObject.transform.position = originPos; //카메라 원위치
        letsShake = false; //그만 흔들어제끼기 (한 번만 흔들기)
    }

}
