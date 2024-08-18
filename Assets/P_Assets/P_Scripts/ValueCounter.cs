using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueCounter : MonoBehaviour
{
    public int totalValue;
    public bool isInside; // 아이템이 안에 있는지 확인

    void Start()
    {
      
    }

    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        // 안에 들어있는것이 아이템이면
        if (other.CompareTag("Item") || other.CompareTag("Doublehand"))
        {
            Item item = other.GetComponent<Item>();

            // 이 오브젝트 콜라이더에 있는 모든 아이템 벨류의 합산
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
