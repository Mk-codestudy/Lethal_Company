using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPod : MonoBehaviour // droppod 가 dropstart 포인트에서 dropendpoint까지 낙하를 lerp를 통해 구현
{

    public Transform startPoint;
    public Transform endPoint;
    public float totalDropDuration = 7f; // 총 낙하할 시간 
    public float speed; // 초기 속도
    public float easing; // 감속 정도
    public GameObject shover;

    public GameObject item1; // pod에서 생성될 아이템
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;

    public Transform regenPos1;
    public Transform regenPos2;
    public Transform regenPos3;
    public Transform regenPos4;
    private bool hasDropped = false; // 도착 여부를 확인


    private float startTime; // 낙하 시작 시간
    private Vector3 velocity;      // 현재 속도

    void Start()
    {
        transform.position = startPoint.position; // 초기 위치는 startpoint의 위치

        startTime = Time.time; // 시작부터 현재까지의 시간을 기록

                               // Time.time 현재 시간

        velocity = (endPoint.position - startPoint.position) * speed * Time.deltaTime;

        Rigidbody rb1 = item1.GetComponent<Rigidbody>();
        Rigidbody rb2 = item2.GetComponent<Rigidbody>();
        Rigidbody rb3 = item3.GetComponent<Rigidbody>();
        Rigidbody rb4 = item4.GetComponent<Rigidbody>();

       

    }

    void Update()
    {
        float elapsedTime = (Time.time - startTime) / totalDropDuration; // 경과시간을 계산

        // 낙하가 끝났으면 elapsedTime을 1로 제한
        if (elapsedTime > 1f)
        {
            elapsedTime = 1f;
        }

        float easedTime = Mathf.Pow(elapsedTime, easing); // 얼마나 감속 비율 계산
        float currentSpeed = Mathf.Lerp(speed, 0, elapsedTime); // 위에서 계산된 감속 비율 만큼 시작 속도에서 0까지 감속한다.

        velocity = Vector3.down * currentSpeed;  // 속도 벡터를 현재 속도로 계산        
        Vector3 displacement = velocity * Time.deltaTime;  // 위에서 구한 속도 * Time.deltatime 을 곱하여 deltatime 동안 이동한 거리를 구함        
        transform.position += displacement; // 위치 업데이트

        if(transform.position.y <= endPoint.position.y) // transform 의 y 축값이 도착지점의 y값보다 크거나 같아졌는지로 도착했는지를 확인
        {
            transform.position = endPoint.position; 


            if (!hasDropped)
            {
                hasDropped = true; // 도착 후 한 번만 실행되도록 설정

                Invoke("InstantiateItem", 1f); // 1초 후 shover 인스턴스화
            }

        }
        
    }

    public void InstantiateItem() // 아이템들을을 지정된 위치에 생성
    {
        Instantiate(item1, regenPos1.position, regenPos1.rotation); // 아이템을 각 리젠 장소에서 생성
        Instantiate(item2, regenPos2.position, regenPos2.rotation); // 
        Instantiate(item3, regenPos3.position, regenPos3.rotation); // 
        Instantiate(item4, regenPos4.position, regenPos4.rotation); //
    }
}
