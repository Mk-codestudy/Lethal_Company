using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [Header("���⿡ �÷��̾� ���ӿ�����Ʈ")]
    public GameObject player; 

    void Start()
    {
        
    }

    void Update()
    {
        if (player != null)
        {
            //Ÿ���� ��ġ�� �÷��̾��� ��ġ��
            transform.position = player.transform.position;
        }
        else
        {
            print("EnemyTarget: player ���� ����!!!");
        }
    }
}
