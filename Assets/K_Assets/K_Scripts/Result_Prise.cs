using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_Prise : MonoBehaviour
{
    [Header("��ü ����(A~F)")]
    public string grade = "F";
    [Header("�� ��ü ��ũ��")]
    public int totalScrab = 0;
    [Header("�Լ��� ���� ��ũ��")]
    public int haveScrab = 0;

    [Header("UI�ݿ�")]
    public Text total_Text;
    public Text have_Text;
    public Text grade_Text;


    void Start()
    {
        total_Text.text = totalScrab.ToString();
        have_Text.text = haveScrab.ToString();
        grade_Text.text = grade;
    }

    private void Update()
    {
        if (GameManager_Proto.gm.playerHP < 0) //����ϼ�����
        {
            have_Text.text = "0";
            grade_Text.text = "F";
        }
    }
}
