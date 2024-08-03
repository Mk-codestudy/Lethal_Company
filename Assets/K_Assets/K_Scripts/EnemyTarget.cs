using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [Header("여기에 플레이어 게임오브젝트")]
    public GameObject player; 

    void Start()
    {
        
    }

    void Update()
    {
        if (player != null)
        {
            //타겟의 위치를 플레이어의 위치로
            transform.position = player.transform.position;
        }
        else
        {
            print("EnemyTarget: player 변수 누락!!!");
        }
    }
}
