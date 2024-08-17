using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMeny : MonoBehaviour
{
    public Text con_text;
    public Image con_img;
    public Text menu_text;
    public Image menu_img;
    public Text exit_text;
    public Image exit_img;

    Color32 orange = new Color32(255, 98, 0, 255);
    Color32 alb = new Color32(0, 0, 0, 0);

    AudioSource hoverSound; //마우스 올렸을 때 틀릴 사운드.

    void Start()
    {
        hoverSound = GetComponent<AudioSource>(); //오디오 소스 캐싱
    }

    public void ContinueInColor()
    {
        con_img.color = orange;
        con_text.color = Color.black;
        //사운드 재생
        hoverSound.Play();
    }

    public void ContinueOutColor()
    {
        con_img.color = alb;
        con_text.color = orange;
    }
    //

    public void MenuInColor()
    {
        menu_img.color = orange;
        menu_text.color = Color.black;
        //사운드 재생
        hoverSound.Play();
    }

    public void MenuOutColor()
    {
        menu_img.color = alb;
        menu_text.color = orange;
    }

    //
    public void ExitInColor()
    {
        exit_img.color = orange;
        exit_text.color = Color.black;
        //사운드 재생
        hoverSound.Play();
    }

    public void ExitOutColor()
    {
        exit_img.color = alb;
        exit_text.color = orange;
    }

    public void Contin()
    {
        GameManager_Proto.gm.Continue();
    }

    public void GotoMain()
    {
        SceneManager.LoadScene(0);
    }

    public void GoExit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#elif UNITY_STANDALONE
        //2. 어플리케이션일 경우
        Application.Quit();

#endif 
    }
}
