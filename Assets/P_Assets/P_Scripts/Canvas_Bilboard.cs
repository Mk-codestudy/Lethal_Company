using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Bilboard : MonoBehaviour
{
    public Canvas itemCanvas; // ĵ������ ������
    public Camera mainCamera;



    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {

        if (itemCanvas != null)
        {
            Vector3 dir = itemCanvas.transform.position - Camera.main.transform.position;  // ����ī�޶�(player) �� �ٶ󺸴� ���� ����

            Quaternion lookRotation = Quaternion.LookRotation(dir); // ī�޶� ���� �������� �ٶ󺸴� rotation

            itemCanvas.transform.rotation = lookRotation;

        }

        //if (itemCanvas != null)
        //{
        //    Vector3 dir = itemCanvas.transform.position - mainCamera.transform.position;  // ����ī�޶�(player) �� �ٶ󺸴� ���� ����

        //    Quaternion lookRotation = Quaternion.LookRotation(dir); // ī�޶� ���� �������� �ٶ󺸴� rotation

        //    itemCanvas.transform.rotation = lookRotation;

        //}






    }
}
