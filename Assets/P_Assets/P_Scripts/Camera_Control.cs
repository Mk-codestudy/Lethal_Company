using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Camera_Control : MonoBehaviour
{
    //�迭(Array)
    //public Transform[] camPositions = new Transform[2];

    //����Ʈ(List)
    public List<Transform> camList = new List<Transform>();


    public Follow_Cam followCam;

    void Start()
    {
        
        followCam = Camera.main.gameObject.GetComponent<Follow_Cam>();

        ChangeCamTarget(0); 
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeCamTarget(0); // 1��Ī ī�޶�
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeCamTarget(1); // 3��Ī ī�޶�
        }
    }

    void ChangeCamTarget(int targetNum)
    {
        if (followCam != null)
        {

            // ���� ī�޶��� folllowcamera Ŭ������ �մ� targetNum����Ҹ� �ִ´�
            followCam.target = camList[targetNum];
        }
    }

}
