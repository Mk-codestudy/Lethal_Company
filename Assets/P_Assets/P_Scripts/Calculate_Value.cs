using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculate_Value : MonoBehaviour // stay �� �������� ��������
{


    [Header("result�� ����� text��")]
    public Text totalValueText;
    public Text totaltotalValueText;
    public Text grade_Text;

    public Item item; // �׸�
    public int totalValue;
    public int totaltotalValue; // ��¥ ���� ��ġ
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
        // Ʈ���� ������ ���� ������Ʈ�� "Item" �Ǵ� "Doublehand" �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Item") || other.CompareTag("Doublehand"))
        {
            Item item = other.GetComponent<Item>();

            // �������� �ְ� ����� ���� �ȵǾ��ٸ�
            if (item != null && item.isCalculated)
            {
                // ���� �ջ��ϰ�
                totalValue += item.itemValue;
                // �ش� �������� ���Ǿ��ٰ� ǥ���Ѵ�.
                item.isCalculated = false;
                UpdateUI();
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    // Ʈ���� �������� ���� ������Ʈ�� "Item" �Ǵ� "Doublehand" �±׸� ������ �ִ��� Ȯ��
    //    if (other.CompareTag("Item") || other.CompareTag("Doublehand"))
    //    {
    //        Item item = other.GetComponent<Item>();

    //        if (item != null)
    //        {
    //            Debug.Log("�������� �������ϴ�.");
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
