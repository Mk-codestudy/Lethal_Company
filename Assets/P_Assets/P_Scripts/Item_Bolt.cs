using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bolt : Item
{

    private void Awake()
    {
        itemName = "볼트";
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
        base.ShowItemInfo(); // 부모에 있는 ShowItemInfo()  함수 실행
    }

    public override void HideItemInfo()
    {
        base.HideItemInfo(); // 부모에 있는 HideItemInfo() 함수 실행
    }
}
