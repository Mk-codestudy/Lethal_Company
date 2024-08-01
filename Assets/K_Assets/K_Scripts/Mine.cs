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

    bool soundOn = false;


    void Update()
    {
        //ringTime마다 삐빅 소리를 낸다.
        if (currenttime < ringTime)
        {
            currenttime += Time.deltaTime;
        }
        else
        {
            //오디오 재생
            currenttime = 0;
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        //print("CollisonEnter!"); //감지!
        if (collision.gameObject.name.Contains("Player")) //감지된 게임오브젝트 이름에 플레이어가 있니?
        {
            rb = collision.gameObject.GetComponent<Rigidbody>(); //찾은 오브젝트 리지드바디 가져오기...
            print("Debug_리지드바디 누구 :" + rb.gameObject.name);

            Invoke("Explosion", 0.5f);
        }         
    }

    public void Explosion()
    {
        if (rb != null) //있으면...
        {
            rb.AddExplosionForce(800, transform.position, radius, 60); //(폭발 파워, 폭발의 중심위치, 각도, 위로 튀어오를 거리값)
                                                                       //플레이어 사망 판정
                                                                       //파티클 뻥
                                                                       //지뢰는 사라짐

            Destroy(gameObject);
        }
        else
        {
            print("인식된 Rigidbidy 없음!");
        }
    }
}


