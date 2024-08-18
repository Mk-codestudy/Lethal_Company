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
    public GameObject mainCam; // 시네마때 삭제 , 생성하기 위해 만든 빈칸들 인스펙터에서 채우면됨
    public GameObject subCam;
    public GameObject statusUi;
    public GameObject timeUI;


    //포드 음악 4개 

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
        subCam.GetComponent<AudioListener>().enabled = false; //오디오 리스너 충돌나서 설정했어요! -민경-
        mainCam.gameObject.SetActive(true);
        statusUi.SetActive(true);
        timeUI.SetActive(true);

        subCam.SetActive(false);
      
    }


    void Update()
    {
        if (isStartCinema)
        {
            subCam.GetComponent<AudioListener>().enabled = true; //오디오 리스너 충돌나서 설정했어요! -민경-

            // 시네머신 시간이 총 시네머신의 시간보다 크다면( 즉 , 시네머신이 끝나면)
            if (director.time >= director.duration)
            {
                director.Stop();
                subCam.SetActive(false); 
                mainCam.SetActive(true);

                statusUi.SetActive(true);
                timeUI.SetActive(true);
                isStartCinema = false;
                FinishCineMachine();

               


                subCam.GetComponent<AudioListener>().enabled = false; //오디오 리스너2222
            }
        }
    }

    public void StartCineMachine()
    {
        director.Play(); // 시네머신 스타트
        isStartCinema = true;

        subCam.SetActive(true);
        mainCam.SetActive(false); // 메인 카메라 오프
        statusUi.SetActive(false);
        timeUI.SetActive(false);
    }

    //시네마틱이 끝나면 캐릭터가 다시 움직이게 (cinematic -> normal 로)
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
