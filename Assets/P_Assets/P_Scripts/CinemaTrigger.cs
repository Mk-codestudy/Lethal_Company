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
        // ���� ����� �÷��̾��ϴ�
        if (other.transform.tag == "Player")
        {
            Debug.Log("�÷��̾ ��ҽ��ϴ�.");
            CinemaManager.instance.StartCineMachine();

            pod.SetActive(true);
            gameObject.SetActive(false);

            //�ó׸�ƽ�̸� �������̰� ���߱�            
            PlayerMove PlayerStop = other.GetComponent<PlayerMove>();

            if(other != null)
            {
                // �ó׸�ƽ���� �ٲ�
                PlayerStop.currentState = PlayerState.Cinematic;
            }         


        }
    }
}
