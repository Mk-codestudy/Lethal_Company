using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class K_Clock : MonoBehaviour
{
    public int time = 8;
    public int min = 0;

    public float currenttime;

    public bool ispm = false;


    [Header("n�ʸ��� �ð� �ٲ��")]
    public float countmin = 15f;

    [Header("�ٲ� �ð��� m�� �þ��")]
    public int stackmin = 15;


    public Text timetext;
    public Text ampmtext;


    void Start()
    {
        
    }

    void Update()
    {
        currenttime += Time.deltaTime;

        if (currenttime > countmin) //n�ʸ���
        {
            min += stackmin; //m�о� �ð� �þ��
            currenttime = 0;
        }

        if (min > 60) //60�� ���� ������
        {
            time += 1; //1�ð� �����ϰ�
            min = min % 60; //������ �� �����ϰ� ��Ƶα�
        }

        if (!ispm && time == 13) //13�ð� �Ǹ� ���ķ� �ٲ��
        {
            time = 1;
            ispm = true;
        }

        if (time < 10) //�ð��� �� �ڸ����϶�
        {
            if (min == 0) //���� 0���� ��(������ ��)
            {
                timetext.text = "0" + time + " : " + "00";
            }
            else if (min < 10) //���� �� �ڸ� ���� ��
            {
                timetext.text = "0" + time + " : " + "0"+min;
            }
            else
            {
                timetext.text = "0" + time + " : " + min;
            }
        }
        else //�� �ڸ����϶�
        {
            if (min == 0)
            {
                timetext.text = time + " : " + "00";
            }
            else if (min < 10) //���� �� �ڸ� ���� ��
            {
                timetext.text = time + " : " + "0" + min;
            }
            else
            {
                timetext.text = time + " : " + min;
            }
        }

        if (!ispm)
        {
            ampmtext.text = "AM";
        }
        else
        {
            ampmtext.text = "PM";
        }
    }
}
