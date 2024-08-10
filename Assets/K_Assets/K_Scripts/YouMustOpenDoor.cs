using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouMustOpenDoor : MonoBehaviour
{
    [Header("n�� �ڿ� ���� ����α�")]
    public float timer = 15f;
    float currenttime;
    public ShipDoor shipdoor;

    void Update()
    {
        if (!shipdoor.isDoorOpen)
        {
            currenttime += Time.deltaTime;
            if (currenttime > timer)
            {
                shipdoor.isDoorOpen = true;
                currenttime = 0;
            }
        }
        else
        {
            currenttime = 0;
        }
    }
}
