using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shover : Item
{
    public PlayerMove playermove;

    private void Awake()
    {
        itemName = "��";
        itemValue = UnityEngine.Random.Range(20, 50);

    }



    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

       // // ����, �÷��̾ �ֿ� �������� �̸��� Shover�� �� �ִٸ�
       //if(playermove.newItem.name.Contains("Shover") || playermove.selectedItem.name.Contains("Shover"))
       // {
       //     // ���� ȸ����
       //     Vector3 shoverVector = new Vector3(3.047f, -240.752f, 1.705f);

       //     Quaternion shoverRotation = Quaternion.Euler(shoverVector);
       // }


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
