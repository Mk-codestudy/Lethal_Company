using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public GameObject[] inventory;
    public Image[] uiSprite = new Image[4]; //     ITME의 스프라이트 이미지를 여기에 저장
   


    private int selectedIndex = -1;

    public void Start()
    {     
      
       inventory = new GameObject[4];
        

       
        
    }

    public void Update()
    {   

    }


    public void AddItem(GameObject item) // slots 안에 게임 오브젝트를 넣는 함수. player에서 호출함
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item; // 비어있는 칸에 아이템 저장
                UpdateSprite(i);

                break;
            }
            else
            {
                Debug.Log("아이템창이 꽉 찼습니다");
            }
        }

    }

   

    public GameObject SelectItem(int index)  // 배열 아이템 가져오기 holditem 변수를 통해 
    {
        if (index >= 0 && index < inventory.Length)
        {
            selectedIndex = index; // 선택된 인덱스를 selectedindex 에 저장
            UpdateSprite(index);
            return inventory[index];
        }
        return null;
    }





    public void RemoveItem(int index) // 배열에서 아이템을 제거하는 함수, 플레이어가 아이템을 버릴때 이 함수도 같이 실행해야 함
    {
        if (index >= 0 && index < inventory.Length)
        {
            inventory[index] = null; // 배열에서 해당 아이템을 null로 설정
            UpdateSprite(index);
        }
    }



    // 아이템 스프라이트를 업데이트하는 함수
    public void UpdateSprite(int index)
    {
        for(int i = 0; i < uiSprite.Length; i++)
        {
            // 인벤토리 배열이 비어있지 않다면
            if (inventory[i] != null)
            {
                // 배열 안에 있는 게임 오브젝트의 item 컴포넌트를 가져온다.
                Item item = inventory[i].GetComponent<Item>();  
                
                if(item != null)
                {
                    uiSprite[i].sprite = item.itemSprite;
                    uiSprite[i].gameObject.SetActive(true);
                }
                else // 아이템칸이 비어있다면
                {
                    Debug.Log("아이템칸이 비어있습니다.");

                    uiSprite[i].sprite = null;
                    uiSprite[i].gameObject.SetActive(false);

                }
            }
            else // 인벤토리 배열이 비어있다면 // 필요한가이게?
            {
                uiSprite[i].sprite = null;
                uiSprite[i].gameObject.SetActive(false);
            }
        }









        //if (index >= 0 && index < inventory.Length)
        //{
        //    if (inventory[index] != null && uiSprite[index] != null)
        //    {
        //        Item item = inventory[index].GetComponent<Item>();

        //        if(item != null)
        //        {
        //            uiSprite[index].sprite = item.itemSprite; // 아이템이 비어있지 않다면 아이템의 스프라이트가 uispirte 의 sprite 이다
        //        }
        //        else
        //        {
        //            Debug.Log(" item 배열이 비어있습니다.");

        //            uiSprite[index].sprite = null;
        //        }
        //    }
        //    else
        //    {
        //        if (uiSprite[index] != null)
        //        {
        //            uiSprite[index].sprite = null; // 빈 슬롯의 경우 스프라이트 제거
        //        }
        //    }
        //}
    }



}


    







   