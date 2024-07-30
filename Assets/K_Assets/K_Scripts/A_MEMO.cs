using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class A_MEMO : MonoBehaviour
{



    void Start()
    {
        
    }

    void Update()
    {
    }

    //움직임 구현 코드 //자주쓸것같음

    public Transform target;
    CharacterController cc; //이동
    public float spd = 1.0f; //속도

    void EnemyMove()
    {
        float h = target.position.x - transform.position.x; //좌우
        float v = target.position.z - transform.position.z; //상하

        Vector3 dir = new Vector3(h, 0, v); //각 축을 조립해서 위치선정

        cc.Move(dir.normalized * spd * Time.deltaTime);

    }

}
