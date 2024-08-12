using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MakePlayerPositionAlpha : MonoBehaviour
{
    public GameObject player;

    public K_Clock clock;

    void Start()
    {
        if (clock.time == 8 && clock.min == 0)
        {
            player.transform.position = new Vector3(-69.5f, 5.14f, -53.3f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            player.transform.position = new Vector3(34.6f, 11.5f, -107.4f);
        }
    }

    void Update()
    {
        
    }
}
