using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaTrigger : MonoBehaviour
{

    public GameObject pod;


    public void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("�÷��̾ ��ҽ��ϴ�.");
            CinemaManager.instance.StartCineMachine();

            pod.SetActive(true);
            gameObject.SetActive(false);


        }
    }
}
