using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items; // �迭�� �κ��丮 ����
    public int inventorySize = 20; // �ִ� ������ ��



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
              
                return true; // ������ �߰� ����
                         
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
                Debug.Log("������ ����: " + items[index].itemName);
                items[index] = null;
            }

        }
    }


    void Update()
    {

    }


}