using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Proto : MonoBehaviour
{
    //prototype를 위한 게임매니저.
    //알파 베타로 넘어갈때마다 필요한 함수만 복붙하기.

    //protype에서 구현되어야 할 사항

    //1. 플레이어 HP 감소 ~ 사망
    [Header("플레이어 스테이터스 관련")]
    public float playerHP = 100;
    public float playerSP = 100;

    [Header("삽 데미지")]
    public float damage = 15;

    //2. 적 HP 감소 ~ 사망
    //적이 여럿이라서 하나만 만들면 안되지만 프로토타입에서 구현할 몹 중 HP있는 놈은 덤퍼 뿐
    public float dumperHP = 100;

    public float enumDamage = 30; //몹 한번 공격할때마다 입는 데미지량



    static public GameManager_Proto gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
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
        //플레이어 사망 판정 
        if (playerHP <= 0)
        {
            PlayerDead();
        }
    }


    //플레이어 사망 함수
    public void PlayerDead()
    {
        //플레이어 레그돌 실행
        //각종 플레이어 기능(이동, 그랩, 기타등등...) 상실
        //카메라 3인칭으로 전환
        //생체 신호 비활성화 UI
    }

    //플레이어가 때리는 함수
    //플레이어가 삽으로 때리는 스크립트에 gm.playerhit(dumperHP) 적는 식으로 사용
    public void PlayerHit(float EnumHP)
    {
        EnumHP -= 15; //데미지 가하기

    }
    public void PlayerOnDamaged()
    {
        //카메라 뒤흔들며 아픈 연출
        //눈앞에 보이는 플레이어 모델링 손이 움직임
    }


    //적이 때리는 함수
    public void AnemHit()
    {
         playerHP -= enumDamage; //적 데미지만큼 플레이어 HP 차감
    }


}
