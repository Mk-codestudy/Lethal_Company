using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProtoUI_main : MonoBehaviour
{
    public Text startText;
    public Image startimage;
    public Text endText;
    public Image endimage;

    Color32 orange = new Color32(255, 98, 0, 255);

    AudioSource hoverSound; //���콺 �÷��� �� Ʋ�� ����.

    private void Start()
    {
        hoverSound = GetComponent<AudioSource>(); //����� �ҽ� ĳ��
    }


    #region ������ �÷����� �� ���ϰ� �ϴ� �ڵ��
    public void StartPointColor()
    {
        startimage.color = orange;
        startText.color = Color.black;
        //���� ���
        hoverSound.Play();
    }

    public void StartPointOutColor()
    {
        startimage.color = Color.black;
        startText.color = orange;
    }

    public void EndPointColor()
    {
        endimage.color = orange;
        endText.color = Color.black;
        //���� ���
        hoverSound.Play();
    }
    public void EndPointOutColor()
    {
        endimage.color = Color.black;
        endText.color = orange;
    }
    #endregion

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#elif UNITY_STANDALONE
        //2. ���ø����̼��� ���
        Application.Quit();

#endif 
    }

}
