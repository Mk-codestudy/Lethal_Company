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

    //������ ���� �ڵ� //���־��Ͱ���

    public Transform target;
    CharacterController cc; //�̵�
    public float spd = 1.0f; //�ӵ�

    void EnemyMove()
    {
        float h = target.position.x - transform.position.x; //�¿�
        float v = target.position.z - transform.position.z; //����

        Vector3 dir = new Vector3(h, 0, v); //�� ���� �����ؼ� ��ġ����

        cc.Move(dir.normalized * spd * Time.deltaTime);

    }

}
