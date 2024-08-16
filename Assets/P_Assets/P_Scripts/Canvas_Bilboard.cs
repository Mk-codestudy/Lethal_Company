using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Bilboard : MonoBehaviour
{
    public Canvas itemCanvas; // ĵ������ ������



    void Start()
    {
        
    }

    void Update()
    {

        if (itemCanvas != null)
        {
            Vector3 dir = itemCanvas.transform.position - Camera.main.transform.position;  // ����ī�޶�(player) �� �ٶ󺸴� ���� ����
            
            Quaternion lookRotation = Quaternion.LookRotation(dir); // ī�޶� ���� �������� �ٶ󺸴� rotation
                                 
            itemCanvas.transform.rotation = lookRotation;



        }






    }
}
