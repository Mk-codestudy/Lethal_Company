using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bolt : Item
{

    private void Awake()
    {
        itemName = "��Ʈ";
        itemValue = Random.Range(60, 150);


    }
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
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
