using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMove;

public class CinemaTrigger : MonoBehaviour
{

    public GameObject pod;
    public GameObject PlayerStop;


    public void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // 닿은 대상이 플레이어일대
        if (other.transform.tag == "Player")
        {
            Debug.Log("플레이어가 닿았습니다.");
            CinemaManager.instance.StartCineMachine();

            pod.SetActive(true);
            gameObject.SetActive(false);

            //시네마틱이면 못움직이게 멈추기            
            PlayerMove PlayerStop = other.GetComponent<PlayerMove>();

            if(other != null)
            {
                // 시네마틱으로 바꿈
                PlayerStop.currentState = PlayerState.Cinematic;
            }         


        }
    }
}
