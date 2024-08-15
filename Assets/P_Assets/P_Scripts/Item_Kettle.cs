using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Kettle : Item // // Monobehaviour -> Item - > Kettle : item 으로 상속받음
{
    //public Vector3 itemScale = new Vector3(0.06f, 0.06f, 0.06f); // 주전자가 더럽게 커서 스케일을 줄이면 캐릭터와 이동하는데 괴리가 생겨서 스크립트상으로 조절




    private void Awake()
    {
        itemName = "주전자";
        itemValue = Random.Range(100, 200);
    }

        public override void Start()
    {
        base.Start(); // 부모의 start 함수를 호출해서 씀

       // Transform currentTransform = transform; // 현재 이 아이템의 transform, 컴포넌트를 가져옴
        //transform.localScale = itemScale; // 로컬 스케일 조정 그리고 인스펙터상에서 스케일을 1이나 부모의 스케일과 동일하게 함. 그러면 이동의 괴리를 없앨수잇음



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





   