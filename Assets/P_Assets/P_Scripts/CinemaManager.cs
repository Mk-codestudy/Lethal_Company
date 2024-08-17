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
    public GameObject mainCam; // 시네마때 삭제 , 생성하기 위해 만든 빈칸들 인스펙터에서 채우면됨
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

            if (director.time >= director.duration)
            {
                director.Stop();
                subCam.SetActive(false); 
                mainCam.SetActive(true);

                statusUi.SetActive(true);
                timeUI.SetActive(true);
                isStartCinema = false;
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
}
