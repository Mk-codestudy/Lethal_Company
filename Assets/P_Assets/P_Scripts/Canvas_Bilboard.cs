using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Bilboard : MonoBehaviour
{
    public Canvas itemCanvas; // 캔버스에 빌보드



    void Start()
    {
        
    }

    void Update()
    {

        if (itemCanvas != null)
        {
            Vector3 dir = itemCanvas.transform.position - Camera.main.transform.position;  // 메인카메라(player) 를 바라보는 방향 벡터
            
            Quaternion lookRotation = Quaternion.LookRotation(dir); // 카메라를 보는 방향으로 바라보는 rotation
                                 
            itemCanvas.transform.rotation = lookRotation;



        }






    }
}
