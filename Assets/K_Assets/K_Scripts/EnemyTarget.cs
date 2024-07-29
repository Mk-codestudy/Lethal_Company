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
        //타겟의 위치를 플레이어의 위치로
        transform.position = player.transform.position;
    }
}
