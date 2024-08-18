using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueCounter : MonoBehaviour
{
    public int totalValue;
    public bool isInside; // �������� �ȿ� �ִ��� Ȯ��

    void Start()
    {
      
    }

    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        // �ȿ� ����ִ°��� �������̸�
        if (other.CompareTag("Item") || other.CompareTag("Doublehand"))
        {
            Item item = other.GetComponent<Item>();

            // �� ������Ʈ �ݶ��̴��� �ִ� ��� ������ ������ �ջ�
            totalValue += item.itemValue; 
        }

              
    }

    private void OnTriggerExit(Collider other)
    {
        Item item = other.GetComponent<Item>();

        if (item != null && isInside)
        {
            isInside = false;
            totalValue -= item.itemValue;
        }
    }


}
