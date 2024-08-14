using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    //지뢰

    //밟으면 터짐
    //주기적으로 삐빅 소리를 냄

    [ Range(3.0f, 10.0f)]
    public float ringTime = 6.0f;
    float currenttime = 0;

    public float radius = 10f;

    public float acTime = 0;
    public float activateTime = 0.5f;

    Rigidbody rb;

    //bool soundOn = false;

    //파티클
    public GameObject explosionparticle;

    //사운드
    AudioSource minesound;
    public AudioClip[] minesoundclip;

    public GameObject lights;


    private void Start()
    {
        explosionparticle.SetActive(false);
        minesound = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        //ringTime마다 삐빅 소리를 낸다.
        if (currenttime < ringTime)
        {
            currenttime += Time.deltaTime;
        }
        else
        {
            //빛이 번쩍
            //오디오 재생
            minesound.clip = minesoundclip[0];
            minesound.Play();
            currenttime = 0;
            lights.SetActive(true);
            Invoke("Offlight", 0.2f);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        //print("CollisonEnter!"); //감지!
        if (collision.gameObject.name.Contains("Player")) //감지된 게임오브젝트 이름에 플레이어가 있니?
        {
            rb = collision.gameObject.GetComponent<Rigidbody>(); //찾은 오브젝트 리지드바디 가져오기...
            Debug.Log("Debug_리지드바디 누구 :" + rb.gameObject.name);

            collision.gameObject.GetComponent<CharacterController>().enabled = false;

            Invoke("AudioOn", 0); //소리 재생

            Invoke("Explosion", 0.5f);
        }         
    }

    public void AudioOn()
    {
        minesound.clip = minesoundclip[1];
        minesound.Play();
        currenttime = 0;
    }

    public void Explosion()
    {
        if (rb != null) //있으면...
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddExplosionForce(750, transform.position, radius, 15); //(폭발 파워, 폭발의 중심위치, 각도, 위로 튀어오를 거리값)
            explosionparticle.SetActive(true);
            Destroy(gameObject.transform.GetChild(0).gameObject);//지뢰 몸은 사라짐
            gameObject.GetComponent<BoxCollider>().enabled = false; //콜라이더 비활성화
            minesound.enabled = false; //사운드도 꺼
            GameManager_Proto.gm.PlayerDead();//플레이어 사망 판정
        }
        else
        {
            Debug.LogWarning("mine :: 인식된 Rigidbidy 없음!");
        }
    }

    public void Offlight()
    {
        lights.SetActive(false);
    }

}


