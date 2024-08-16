using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shover : Item
{
    public PlayerMove playermove;

    private void Awake()
    {
        itemName = "삽";
        itemValue = UnityEngine.Random.Range(20, 50);

    }



    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

       // // 만약, 플레이어가 주운 아이템의 이름에 Shover가 들어가 있다면
       //if(playermove.newItem.name.Contains("Shover") || playermove.selectedItem.name.Contains("Shover"))
       // {
       //     // 삽의 회전값
       //     Vector3 shoverVector = new Vector3(3.047f, -240.752f, 1.705f);

       //     Quaternion shoverRotation = Quaternion.Euler(shoverVector);
       // }


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
