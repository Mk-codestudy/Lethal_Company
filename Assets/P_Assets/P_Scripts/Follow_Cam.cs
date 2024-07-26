using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Cam : MonoBehaviour
{
    public Transform target;
    public float rotX;


    Transform player;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if(target != null && player != null)
        {
            transform.position = target.position; // ��ġ�� Ÿ���� ��ġ

            //Vector3 dir = (player.position - transform.position).normalized; // ī�޶� Ÿ���� �ٶ󺸴� ����
            transform.forward = player.forward; // ���� ������ Ÿ���� �ٶ󺸴� ������ ��

            transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y, transform.eulerAngles.z); //rotx�� �÷��̾�� ���޹��� ��
        }
    }

}

