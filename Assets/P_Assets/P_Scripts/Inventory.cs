using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public GameObject[] inventory;
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
            return inventory[index];
        }
        return null;
    }





    public void RemoveItem(int index) // 배열에서 아이템을 제거하는 함수, 플레이어가 아이템을 버릴때 이 함수도 같이 실행해야 함
    {
        if (index >= 0 && index < inventory.Length)
        {
            inventory[index] = null; // 배열에서 해당 아이템을 null로 설정
        }
    }



}


    







   