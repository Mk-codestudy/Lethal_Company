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
            //����, ���� ���� �ð��� ��ü ���� �ð��� �����ߴٸ�...
            if (director.time >= director.duration)
            {
                //�ó׸ӽ��� �����ϰ� �ʹ�.
                director.Stop();

                //���� ī�޶� Ȱ��ȭ�Ѵ�.
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
