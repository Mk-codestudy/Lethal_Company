using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items; // 배열로 인벤토리 관리
    public int inventorySize = 20; // 최대 아이템 수



    void Start()
    {
        items = new Item[inventorySize];
    }

    public bool AddItem(Item newItem)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
              
                return true; // 아이템 추가 성공
                         
            }
        }
        return false;
    }

    public void RemoveItem(int index)
    {
        if(index >= 0 && index < items.Length)
        {
            if(items[index] == null)
            {
                Debug.Log("아이템 제거: " + items[index].itemName);
                items[index] = null;
            }

        }
    }


    void Update()
    {

    }


}