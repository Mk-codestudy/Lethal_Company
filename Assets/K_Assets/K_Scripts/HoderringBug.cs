using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoderringBug : MonoBehaviour
{
    //비축벌레

    //맵 전역을 돌아다니며 아이템을 줍는다.
    //스크랩이 보이면 한 번에 하나씩 주워간다.
    //양손 아이템은 줍지 못한다.

    //자신이 스폰된 장소를 집으로 삼고 물건을 근처에 모아 둔다.

    // 플레이어와 일정 이상 거리가 붙으면 경계 모드 애니메이팅을 한다.
    // 플레이어와 다시 거리가 벌어지면 탐색 모드로 돌아간다.

    // 플레이어가 너무 가까이 다가오면 공격한다.
    // 플레이어가 자신의 보금자리에 침입하면 공격한다.
    // 플레이어가 자신이 집었던 스크랩을 주우면 공격한다.

    // 삽으로 3방 맞으면 죽는다.

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
