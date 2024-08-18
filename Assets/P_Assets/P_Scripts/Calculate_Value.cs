using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculate_Value : MonoBehaviour // stay 한 아이템의 벨류값을
{


    [Header("result에 출력할 text들")]
    public Text totalValueText;
    public Text totaltotalValueText;
    public Text grade_Text;

    public Item item; // 그릇
    public int totalValue;
    public int totaltotalValue; // 진짜 최종 가치
    public string grade;


    void Start()
    {
        totalValue = 0;
       
    }

    void Update()
    {
        UpdateUI();

        //if (grade = "c" || grade = "b" || grade = "A")
        //{
        //    gameObject.GoalImeage.SetActive(true);
        //}

           
    }
     

    public void UpdateUI()
    {

        if(totalValueText != null)
        {
            totalValueText.text = totalValue.ToString();
        }


        if (totaltotalValueText != null)
        {
            totaltotalValueText.text = "800";
        }

        if (grade_Text != null)
        {
            GradeRank();
            grade_Text.text = grade.ToString();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        // 트리거 영역에 들어온 오브젝트가 "Item" 또는 "Doublehand" 태그를 가지고 있는지 확인
        if (other.CompareTag("Item") || other.CompareTag("Doublehand"))
        {
            Item item = other.GetComponent<Item>();

            // 아이템이 있고 계산이 아직 안되었다면
            if (item != null && item.isCalculated)
            {
                // 값에 합산하고
                totalValue += item.itemValue;
                // 해당 아이템은 계산되었다고 표시한다.
                item.isCalculated = false;
                UpdateUI();
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    // 트리거 영역에서 나간 오브젝트가 "Item" 또는 "Doublehand" 태그를 가지고 있는지 확인
    //    if (other.CompareTag("Item") || other.CompareTag("Doublehand"))
    //    {
    //        Item item = other.GetComponent<Item>();

    //        if (item != null)
    //        {
    //            Debug.Log("아이템이 나갔습니다.");
    //            if (!item.isCalculated)
    //            {
    //                totalValue -= item.itemValue;
    //                item.isCalculated = true;
    //                UpdateUI();
    //            }
    //        }

    //    }
    //}
         



    void GradeRank()
    {
        if(100 >= totalValue && totalValue >= 0)
        {
            grade = "C";
        }
        else if(300 >= totalValue && totalValue >= 101)
        {
            grade = "B";
        }
        else if (totalValue >= 301)
        {
            grade = "A";
        }
        else
        {
            return;
        }

    }




}
