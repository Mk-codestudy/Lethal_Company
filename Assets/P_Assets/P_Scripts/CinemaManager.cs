using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static PlayerMove;

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


    //���� ���� 4�� 

    public PlayableDirector musicDirector1;

    public PlayableDirector musicDirector2;
    public PlayableDirector musicDirector3;
    public PlayableDirector musicDirector4;

    public PlayableDirector musicDirector5;

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

            // �ó׸ӽ� �ð��� �� �ó׸ӽ��� �ð����� ũ�ٸ�( �� , �ó׸ӽ��� ������)
            if (director.time >= director.duration)
            {
                director.Stop();
                subCam.SetActive(false); 
                mainCam.SetActive(true);

                statusUi.SetActive(true);
                timeUI.SetActive(true);
                isStartCinema = false;
                FinishCineMachine();

               


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

    //�ó׸�ƽ�� ������ ĳ���Ͱ� �ٽ� �����̰� (cinematic -> normal ��)
    public void FinishCineMachine()
    {
        PlayerMove playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove != null)
        {
            playerMove.currentState = PlayerState.Normal;
        }


        CinemaTrigger CinemaTrigger = GetComponent<CinemaTrigger>();
        Destroy(CinemaTrigger);

    }

    public void StartMusic()
    {
        musicDirector1.Play();
        musicDirector2.Play(); 
        musicDirector3.Play(); 
        musicDirector4.Play(); 
        musicDirector5.Play(); 
    }

    void StopMusic()
    {
        musicDirector1.Stop();
        musicDirector2.Stop();
        musicDirector3.Stop();
        musicDirector5.Stop();
    }







}
