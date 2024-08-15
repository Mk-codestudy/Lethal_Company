using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMoving : MonoBehaviour
{
    [Header("이착륙 지점 지정")]
    public Transform departPos;
    public Transform landPos;


    [Header("착륙 여부 확인 변수")]
    public bool alreadyLanding;

    [Header("이륙 여부 확인 변수")]
    public bool departing;

    [Header("플레이어 찾기")]
    public GameObject player;

    //착륙 Lerp용 퍼센트
    float landPercent = 0;
    
    //이륙 Lerp용 퍼센트
    float departPercent = 0;


    void Start()
    {
        
    }

    void Update()
    {
        //배가 천천히 랜딩했으면 좋겠다
        //그리고 랜딩 여부는 bool로 체크되어야 한다(중간 저장이 되어야 하므로...)
        //Lerp로 천천히 착륙하기

        if (!departing && !alreadyLanding) //게임 시작시 (아직 착륙하지 않았고, 이륙하지도 않음)
        {
            landPercent += Time.deltaTime * 0.2f ;
            Vector3 result = Vector3.Lerp(departPos.position, landPos.position, landPercent);

            player.GetComponent<CharacterController>().Move(result - transform.position);

            transform.position = result;
            

            if (Vector3.Distance(transform.position, landPos.position) < 0.3f)
            {
                alreadyLanding = true;
            }
        }
        else if (!departing && alreadyLanding) //함선이 착륙했을 때 (이륙하기 전까지)
        {
            transform.position = landPos.position;

        }
        else if (departing)
        {
            landPercent += Time.deltaTime * 0.2f;
            Vector3 result = Vector3.Lerp(landPos.position, departPos.position, departPercent);
            player.GetComponent<CharacterController>().Move(result - transform.position);

            transform.position = result;
        }

    }
}
