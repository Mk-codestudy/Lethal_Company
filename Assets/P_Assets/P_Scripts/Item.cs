using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour // 이 아이템 스크립트를 다른 아이템 스크립트에 상속한다.
{

    public string itemName;
    public int itemValue;

    public TMP_Text itemName_text; // 아이템 이름
    public TMP_Text itemName_value; // 아이템 정보 를 표시
    public Sprite itemSprite; // 아이템의 스프라이트 이미지 

    public Camera mainCamera; 

    public virtual void Start()          


    {
        mainCamera = Camera.main; // 씬에서 메인 카메라를 찾아서 이 변수에 담는다.


        // 아이템 생성과 동시에 ui 비활성화
        if (itemName_text != null)
        {
            itemName_text.gameObject.SetActive(false);
        }
        if (itemName_value != null)
        {
            itemName_value.gameObject.SetActive(false);
        }
    }

    public virtual void Update() // 
    {
        if(itemName_text != null)
        {
            itemName_text.text = itemName; // 아이템 이름표시
        }
        if(itemName_value != null)
        {
            itemName_value.text = itemValue.ToString(); // 가치표시
        }
       
        



        //if (itemName_text != null && itemName_value != null)
        //{
        //    Vector3 dir1 = itemName_text.transform.position - Camera.main.transform.position;  // 메인카메라(player) 를 바라보는 방향 벡터
        //    Vector3 dir2 = itemName_value.transform.position - Camera.main.transform.position; // 메인카메라(player) 를 바라보는 방향 벡터

        //    Quaternion lookRotation1 = Quaternion.LookRotation(dir1); // 카메라를 보는 방향으로 바라보는 rotation
        //    Quaternion lookRotation2 = Quaternion.LookRotation(dir2); // 카메라를 보는 방향으로 바라보는 rotation

        //    itemName_text.transform.rotation = lookRotation1;
        //    itemName_value.transform.rotation = lookRotation2;



        //}
    }


    public virtual void ShowItemInfo() // 자식에 상속하기위해 virtual 로 
    {
        // 우클릭하면 숨겼던 ui 보이게 하기
        if(itemName_text != null)
        {
            itemName_text.gameObject.SetActive(true);
        }
        if(itemName_value)
        {
            itemName_value.gameObject.SetActive(true);
        }
             
        

        Invoke("HideItemInfo", 3f);  // Invoke를 호출할 때 메서드 이름을 문자열로 전달 할 수 있다. 3초 뒤 ui 를 끈다.
    }


    // 3초 뒤에 ui 다시 숨기기
    public virtual  void HideItemInfo()
    {
        if(itemName_text != null)
        {
            itemName_text.gameObject.SetActive(false);
        }
        if(itemName_value != null)
        {
            itemName_value.gameObject.SetActive(false);
        }
        
    }




}
