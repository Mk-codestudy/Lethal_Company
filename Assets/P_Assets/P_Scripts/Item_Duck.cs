using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Duck : Item // Monobehaviour -> Item - > Duck : item 으로 상속받음
{
    private void Awake()
    {
        itemName = "오리";
        itemValue = Random.Range(70, 100);
    }
    public override void Start()
    {
        base.Start(); // 부모의 start 함수를 호출해서 씀
    }

    public override void Update()
    {
        base.Update(); // 부모의 update 함수- Bilboard를 가져와서 구현
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

    
