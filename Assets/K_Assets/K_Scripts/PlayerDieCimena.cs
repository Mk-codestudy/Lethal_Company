using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Windows.WebCam;

public class PlayerDieCimena : MonoBehaviour
{
    public static PlayerDieCimena instance;
    public PlayableDirector director;

    public Camera mainCam;
    public Camera subcam;

    bool isStartCinema;
    public bool isCinemaEnd;

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
        
    }

    void Update()
    {
        if (isStartCinema)
        {
            subcam.gameObject.SetActive(true);
            subcam.GetComponent<AudioListener>().enabled = true;
            //만일, 현재 진행 시간이 전체 진행 시간에 도달했다면...
            if (director.time >= director.duration)
            {
                //시네머신을 중지하고 싶다.
                director.Stop();

                //메인 카메라를 활성화한다.
                //mainCam.gameObject.SetActive(true);
                subcam.gameObject.SetActive(false);
                //subcam.transform.position = GameManager_Proto.gm.closeDoorCamPos.position;
                //subcam.transform.rotation = GameManager_Proto.gm.closeDoorCamPos.rotation;

                isStartCinema = false;
                isCinemaEnd = true;
            }
        }
    }
    public void StartCinemachime()
    {
        director.Play();
        isStartCinema = true;

        Camera.main.gameObject.SetActive(false);

        
    }

}
