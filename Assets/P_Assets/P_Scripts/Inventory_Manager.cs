using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{

    public Item[] slots;  // 4ĭ�� �κ��丮 ����


    public void Start()
    {
       
        slots = new Item[4]; // �κ��丮 �ʱ�ȭ
    }





    // �������� �ݴ��Լ�
    public void AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) // �� ������ ã��
            {
                slots[i] = item; // �������� ���Կ� �߰�

                Debug.Log("�������� �߰��Ǿ����ϴ�. ����: " + i);
                return;
            }
        }
        Debug.Log("������ â�� �� á���ϴ�.");
    }


    public void RemoveItem(int slotIndex)  // �������� ����� �Լ�
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex] = null; // ���Կ��� ������ ����
            Debug.Log("�������� ���ŵǾ����ϴ�. ����: " + slotIndex);
        }
        else
        {
            Debug.Log("�߸��� ���� �ε���: " + slotIndex);
        }
    }



}
