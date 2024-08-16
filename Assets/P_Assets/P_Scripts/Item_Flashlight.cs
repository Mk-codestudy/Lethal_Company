using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Flashlight : Item
{
    private Light flashlight;


    private void Awake()
    {
        itemName = "������";
        itemValue = Random.Range(100, 120);

       
    }

    public override void Start()
    {
        flashlight = GetComponentInChildren<Light>();

        flashlight.enabled = false;

        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public void LightOnOff() // ���� ���� �ִ��ϴ� �Լ�.
    {
        
            if (flashlight != null)
            {
                flashlight.enabled = !flashlight.enabled; // �����ִ� �ϱ�
            }
        
    }


    public override void ShowItemInfo()
    {
        base.ShowItemInfo(); // �θ� �ִ� ShowItemInfo()  �Լ� ����
    }

    public override void HideItemInfo()
    {
        base.HideItemInfo(); // �θ� �ִ� HideItemInfo() �Լ� ����
    }



}
