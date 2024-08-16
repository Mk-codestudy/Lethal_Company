using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public GameObject[] inventory;
    public Image[] uiSprite = new Image[4]; //     ITME�� ��������Ʈ �̹����� ���⿡ ����
   


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
                UpdateSprite(i);

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
            UpdateSprite(index);
            return inventory[index];
        }
        return null;
    }





    public void RemoveItem(int index) // �迭���� �������� �����ϴ� �Լ�, �÷��̾ �������� ������ �� �Լ��� ���� �����ؾ� ��
    {
        if (index >= 0 && index < inventory.Length)
        {
            inventory[index] = null; // �迭���� �ش� �������� null�� ����
            UpdateSprite(index);
        }
    }



    // ������ ��������Ʈ�� ������Ʈ�ϴ� �Լ�
    public void UpdateSprite(int index)
    {
        for(int i = 0; i < uiSprite.Length; i++)
        {
            // �κ��丮 �迭�� ������� �ʴٸ�
            if (inventory[i] != null)
            {
                // �迭 �ȿ� �ִ� ���� ������Ʈ�� item ������Ʈ�� �����´�.
                Item item = inventory[i].GetComponent<Item>();  
                
                if(item != null)
                {
                    uiSprite[i].sprite = item.itemSprite;
                    uiSprite[i].gameObject.SetActive(true);
                }
                else // ������ĭ�� ����ִٸ�
                {
                    Debug.Log("������ĭ�� ����ֽ��ϴ�.");

                    uiSprite[i].sprite = null;
                    uiSprite[i].gameObject.SetActive(false);

                }
            }
            else // �κ��丮 �迭�� ����ִٸ� // �ʿ��Ѱ��̰�?
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
        //            uiSprite[index].sprite = item.itemSprite; // �������� ������� �ʴٸ� �������� ��������Ʈ�� uispirte �� sprite �̴�
        //        }
        //        else
        //        {
        //            Debug.Log(" item �迭�� ����ֽ��ϴ�.");

        //            uiSprite[index].sprite = null;
        //        }
        //    }
        //    else
        //    {
        //        if (uiSprite[index] != null)
        //        {
        //            uiSprite[index].sprite = null; // �� ������ ��� ��������Ʈ ����
        //        }
        //    }
        //}
    }



}


    







   