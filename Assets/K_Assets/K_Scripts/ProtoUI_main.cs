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


    #region 포인터 올렸을때 색 변하게 하는 코드들
    public void StartPointColor()
    {
        startimage.color = orange;
        startText.color = Color.black;
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
        //2. 어플리케이션일 경우
        Application.Quit();

#endif 
    }

}
