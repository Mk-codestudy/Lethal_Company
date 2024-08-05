using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
   public GameObject[] items; // 인벤토리 4칸 
                              // 나중에 화면에 표시되게 빈칸 4개를 만듬

   // public List<GameObject> items = new List<GameObject>(); // 동적 리스트

    public int selectedSlot = 0; // 현재 선택된 슬롯 인덱스

        


    void Start()
    {
        items = new GameObject[4];
    }


    void Update()
    {
     
    }

    public void AddItem(GameObject item) // 아이템을 배열에 추가
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                Debug.Log("아이템이 슬롯 " + i + "에 추가되었습니다.");
                return; // 반복
            }
        }
    }

    public void RemoveItem(GameObject item) // 아이템을 배열에서 제거
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null; // 아이템 제거
                Debug.Log("아이템이 슬롯 " + i + "에서 제거되었습니다.");
                return; // 메서드 종료
            }
        }

        Debug.Log("Item not found in inventory."); // 인벤토리에서 아이템을 찾지 못했을 때의 로그
    }

    // 현재 선택된 슬롯의 아이템을 가져오는 메서드
    public GameObject GetSelectedItem()
    {
        return items[selectedSlot];
    }




    //public void AddItem(GameObject item)  리스트 사용
    //{
    //    if (items.Count < 4) // 최대 4개의 슬롯
    //    {
    //        items.Add(item);
    //        Debug.Log("아이템이 인벤토리에 추가되었습니다.");
    //    }
    //    else
    //    {
    //        Debug.Log("인벤토리가 가득 찼습니다.");
    //    }
    //}

    //public void RemoveItem(GameObject item)
    //{
    //    if (items.Remove(item))
    //    {
    //        Debug.Log("아이템이 인벤토리에서 제거되었습니다.");
    //    }
    //    else
    //    {
    //        Debug.Log("아이템을 찾을 수 없습니다.");
    //    }
    //}
}