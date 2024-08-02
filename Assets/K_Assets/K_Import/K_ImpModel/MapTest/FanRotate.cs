using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotate : MonoBehaviour
{
    float rot;
    public float speed;

    void Update()
    {
        rot += speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(-90, 0, rot);
    }
}
