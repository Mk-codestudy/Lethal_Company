using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Flashlight : Item
{
    private Light flashlight;


    private void Awake()
    {
        itemName = "손전등";
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

    public void LightOnOff() // 불을 껏다 켯다하는 함수.
    {
        
            if (flashlight != null)
            {
                flashlight.enabled = !flashlight.enabled; // 껏다켯다 하기
            }
        
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
