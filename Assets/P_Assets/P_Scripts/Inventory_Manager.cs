using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{

    public Item[] slots;  // 4칸의 인벤토리 슬롯


    public void Start()
    {
       
        slots = new Item[4]; // 인벤토리 초기화
    }





    // 아이템을 줍는함수
    public void AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) // 빈 슬롯을 찾음
            {
                slots[i] = item; // 아이템을 슬롯에 추가

                Debug.Log("아이템이 추가되었습니다. 슬롯: " + i);
                return;
            }
        }
        Debug.Log("아이템 창이 꽉 찼습니다.");
    }


    public void RemoveItem(int slotIndex)  // 아이템을 지우는 함수
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex] = null; // 슬롯에서 아이템 제거
            Debug.Log("아이템이 제거되었습니다. 슬롯: " + slotIndex);
        }
        else
        {
            Debug.Log("잘못된 슬롯 인덱스: " + slotIndex);
        }
    }



}
