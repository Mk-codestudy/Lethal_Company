using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_Prise : MonoBehaviour
{
    [Header("전체 성적(A~F)")]
    public string grade = "F";
    [Header("맵 전체 스크랩")]
    public int totalScrab = 0;
    [Header("함선에 모은 스크랩")]
    public int haveScrab = 0;

    [Header("UI반영")]
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
        if (GameManager_Proto.gm.playerHP < 0) //운명하셧을대
        {
            have_Text.text = "0";
            grade_Text.text = "F";
        }
    }
}
