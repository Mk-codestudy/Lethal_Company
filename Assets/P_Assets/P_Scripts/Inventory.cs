using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
   public GameObject[] items; // �κ��丮 4ĭ 
                              // ���߿� ȭ�鿡 ǥ�õǰ� ��ĭ 4���� ����

   // public List<GameObject> items = new List<GameObject>(); // ���� ����Ʈ

    public int selectedSlot = 0; // ���� ���õ� ���� �ε���

        


    void Start()
    {
        items = new GameObject[4];
    }


    void Update()
    {
     
    }

    public void AddItem(GameObject item) // �������� �迭�� �߰�
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                Debug.Log("�������� ���� " + i + "�� �߰��Ǿ����ϴ�.");
                return; // �ݺ�
            }
        }
    }

    public void RemoveItem(GameObject item) // �������� �迭���� ����
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null; // ������ ����
                Debug.Log("�������� ���� " + i + "���� ���ŵǾ����ϴ�.");
                return; // �޼��� ����
            }
        }

        Debug.Log("Item not found in inventory."); // �κ��丮���� �������� ã�� ������ ���� �α�
    }

    // ���� ���õ� ������ �������� �������� �޼���
    public GameObject GetSelectedItem()
    {
        return items[selectedSlot];
    }




    //public void AddItem(GameObject item)  ����Ʈ ���
    //{
    //    if (items.Count < 4) // �ִ� 4���� ����
    //    {
    //        items.Add(item);
    //        Debug.Log("�������� �κ��丮�� �߰��Ǿ����ϴ�.");
    //    }
    //    else
    //    {
    //        Debug.Log("�κ��丮�� ���� á���ϴ�.");
    //    }
    //}

    //public void RemoveItem(GameObject item)
    //{
    //    if (items.Remove(item))
    //    {
    //        Debug.Log("�������� �κ��丮���� ���ŵǾ����ϴ�.");
    //    }
    //    else
    //    {
    //        Debug.Log("�������� ã�� �� �����ϴ�.");
    //    }
    //}
}