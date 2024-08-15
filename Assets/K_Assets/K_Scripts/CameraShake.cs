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

    IEnumerator CamShake(float duration, float magnitude) //ī�޶� ��鸮�� �ϴ� �ڷ�ƾ, float �۵��ð�, float �۵�����
    {
        float timer = 0;
        originPos = gameObject.transform.position;
        player.transform.SetParent(gameObject.transform);

        while (timer <= duration)
        {
            gameObject.transform.position = Random.insideUnitSphere * magnitude + originPos;
            timer += Time.deltaTime; //�ð� �帣��
            yield return null;
        }

        gameObject.transform.position = originPos; //ī�޶� ����ġ
        letsShake = false; //�׸� ���������� (�� ���� ����)
    }

}
