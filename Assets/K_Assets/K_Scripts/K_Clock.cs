using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class K_Clock : MonoBehaviour
{
    //public static K_Clock instance;  // Singleton 인스턴스

    public int time = 8;
    public int min = 0;

    public float currenttime;

    public bool ispm = false;


    [Header("n초마다 시간 바뀌기")]
    public float countmin = 15f;

    [Header("바뀐 시간은 m분 늘어나기")]
    public int stackmin = 15;


    public Text timetext;
    public Text ampmtext;


    void Awake()
    {
        //// Singleton 패턴을 구현하는 부분
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);  // 씬 전환 시 파괴되지 않도록 설정
        //}
        //else
        //{
        //    Destroy(gameObject);  // 이미 인스턴스가 존재한다면 파괴
        //}
    }

    void Update()
    {
        currenttime += Time.deltaTime;

        if (currenttime > countmin) //n초마다
        {
            min += stackmin; //m분씩 시간 늘어나기
            currenttime = 0;
        }

        if (min >= 60) //60분 지날 때마다
        {
            time += 1; //1시간 증가하고
            min = min % 60; //나머지 분 소중하게 모아두기
        }

        if (!ispm && time == 13) //13시가 되면 오후로 바뀌기
        {
            time = 1;
            ispm = true;
        }

        if (time < 10) //시간이 한 자리수일때
        {
            if (min == 0) //분이 0분일 때(정각일 때)
            {
                timetext.text = "0" + time + " : " + "00";
            }
            else if (min < 10) //분이 한 자리 수일 때
            {
                timetext.text = "0" + time + " : " + "0"+min;
            }
            else
            {
                timetext.text = "0" + time + " : " + min;
            }
        }
        else //두 자리수일때
        {
            if (min == 0)
            {
                timetext.text = time + " : " + "00";
            }
            else if (min < 10) //분이 한 자리 수일 때
            {
                timetext.text = time + " : " + "0" + min;
            }
            else
            {
                timetext.text = time + " : " + min;
            }
        }


        // AM/PM 텍스트 업데이트
        ampmtext.text = ispm ? "PM" : "AM";


        if (ispm && time == 22)
        {
            Debug.Log("제한시간 종료 전 경고!");
        }

        if (ispm && time == 23)
        {
            Debug.Log("제한시간 종료!");
            
        }

    }
}
