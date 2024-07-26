using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Camera_Control : MonoBehaviour
{
    //배열(Array)
    //public Transform[] camPositions = new Transform[2];

    //리스트(List)
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
            ChangeCamTarget(0); // 1인칭 카메라
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeCamTarget(1); // 3인칭 카메라
        }
    }

    void ChangeCamTarget(int targetNum)
    {
        if (followCam != null)
        {

            // 메인 카메라의 folllowcamera 클래스에 잇는 targetNum번요소를 넣는다
            followCam.target = camList[targetNum];
        }
    }

}
