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
            transform.position = target.position; // 위치는 타켓의 위치

            //Vector3 dir = (player.position - transform.position).normalized; // 카메라가 타켓을 바라보는 방향
            transform.forward = player.forward; // 나의 정면이 타겟을 바라보는 방향이 됨

            transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y, transform.eulerAngles.z); //rotx만 플레이어에게 전달받은 값
        }
    }

}

