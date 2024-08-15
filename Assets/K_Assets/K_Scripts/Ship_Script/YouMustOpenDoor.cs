using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouMustOpenDoor : MonoBehaviour
{
    [Header("n�� �ڿ� ���� ����α�")]
    public float timer = 15f;
    float currenttime;
    public ShipDoor shipdoor;

    private void Start()
    {
        Invoke("OpenDoor", 2.0f);
    }

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

    void OpenDoor()
    {
        shipdoor.isDoorOpen = true;
    }
}
