using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinemaManager : MonoBehaviour
{
    public static CinemaManager instance;
    public PlayableDirector director;
    public PlayerMove player;
    bool isStartCinema = false;
    public GameObject mainCam; // �ó׸��� ���� , �����ϱ� ���� ���� ��ĭ�� �ν����Ϳ��� ä����
    public GameObject subCam;
    public GameObject statusUi;
    public GameObject timeUI; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        subCam.GetComponent<AudioListener>().enabled = false; //����� ������ �浹���� �����߾��! -�ΰ�-
        mainCam.gameObject.SetActive(true);
        statusUi.SetActive(true);
        timeUI.SetActive(true);

        subCam.SetActive(false);
      
    }


    void Update()
    {
        if (isStartCinema)
        {
            subCam.GetComponent<AudioListener>().enabled = true; //����� ������ �浹���� �����߾��! -�ΰ�-

            if (director.time >= director.duration)
            {
                director.Stop();
                subCam.SetActive(false); 
                mainCam.SetActive(true);

                statusUi.SetActive(true);
                timeUI.SetActive(true);
                isStartCinema = false;
                subCam.GetComponent<AudioListener>().enabled = false; //����� ������2222
            }
        }
    }

    public void StartCineMachine()
    {
        director.Play(); // �ó׸ӽ� ��ŸƮ
        isStartCinema = true;

        subCam.SetActive(true);
        mainCam.SetActive(false); // ���� ī�޶� ����
        statusUi.SetActive(false);
        timeUI.SetActive(false);
    }
}
