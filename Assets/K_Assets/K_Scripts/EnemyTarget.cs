using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    public GameObject player; 

    void Start()
    {
        
    }

    void Update()
    {
        //Ÿ���� ��ġ�� �÷��̾��� ��ġ��
        transform.position = player.transform.position;
    }
}
