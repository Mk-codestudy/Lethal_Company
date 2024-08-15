using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Flashlight : Item
{
    private Light flashlight;


    private void Awake()
    {
        itemName = "Flashlight";
        itemValue = Random.Range(100, 100);

       
    }




    public override void Start()
    {
        flashlight = GetComponentInChildren<Light>();

        flashlight.enabled = false;
    }

    public override void Update()
    {
        
    }

    public void LightOnOff() // ���� ���� �ִ��ϴ� �Լ�.
    {
        
            if (flashlight != null)
            {
                flashlight.enabled = !flashlight.enabled; // �����ִ� �ϱ�
            }
        
    }



}
