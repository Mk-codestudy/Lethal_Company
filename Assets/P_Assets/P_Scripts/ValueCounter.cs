using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueCounter : MonoBehaviour
{
    public int totalValue;

    void Start()
    {
      
    }

    void Update()
    {
        
    }



    private void OnTriggerStay(Collider other)
    {
        // �ȿ� ����ִ°��� �������̸�
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();

            // �� ������Ʈ �ݶ��̴��� �ִ� ��� ������ ������ �ջ�
            totalValue += item.itemValue; 
        }

              
    }
}
