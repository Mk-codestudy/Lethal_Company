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


    public void AddItem(GameObject item) // slots �ȿ� ���� ������Ʈ�� �ִ� �Լ�. player���� ȣ����
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item; // ����ִ� ĭ�� ������ ����

                break;
            }
            else
            {
                Debug.Log("������â�� �� á���ϴ�");
            }
        }

    }

   

    public GameObject SelectItem(int index)  // �迭 ������ �������� holditem ������ ���� 
    {
        if (index >= 0 && index < inventory.Length)
        {
            selectedIndex = index; // ���õ� �ε����� selectedindex �� ����
            return inventory[index];
        }
        return null;
    }





    public void RemoveItem(int index) // �迭���� �������� �����ϴ� �Լ�, �÷��̾ �������� ������ �� �Լ��� ���� �����ؾ� ��
    {
        if (index >= 0 && index < inventory.Length)
        {
            inventory[index] = null; // �迭���� �ش� �������� null�� ����
        }
    }



}


    







   