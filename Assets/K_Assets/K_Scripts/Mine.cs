using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    //����

    //������ ����
    //�ֱ������� �ߺ� �Ҹ��� ��

    [ Range(3.0f, 10.0f)]
    public float ringTime = 6.0f;
    float currenttime = 0;

    public float radius = 10f;

    public float acTime = 0;
    public float activateTime = 0.5f;

    Rigidbody rb;

    //bool soundOn = false;

    //��ƼŬ
    public GameObject explosionparticle;

    //����
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
        //ringTime���� �ߺ� �Ҹ��� ����.
        if (currenttime < ringTime)
        {
            currenttime += Time.deltaTime;
        }
        else
        {
            //���� ��½
            //����� ���
            minesound.clip = minesoundclip[0];
            minesound.Play();
            currenttime = 0;
            lights.SetActive(true);
            Invoke("Offlight", 0.2f);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        //print("CollisonEnter!"); //����!
        if (collision.gameObject.name.Contains("Player")) //������ ���ӿ�����Ʈ �̸��� �÷��̾ �ִ�?
        {
            rb = collision.gameObject.GetComponent<Rigidbody>(); //ã�� ������Ʈ ������ٵ� ��������...
            Debug.Log("Debug_������ٵ� ���� :" + rb.gameObject.name);

            collision.gameObject.GetComponent<CharacterController>().enabled = false;

            Invoke("AudioOn", 0); //�Ҹ� ���

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
        if (rb != null) //������...
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddExplosionForce(750, transform.position, radius, 15); //(���� �Ŀ�, ������ �߽���ġ, ����, ���� Ƣ����� �Ÿ���)
            explosionparticle.SetActive(true);
            Destroy(gameObject.transform.GetChild(0).gameObject);//���� ���� �����
            gameObject.GetComponent<BoxCollider>().enabled = false; //�ݶ��̴� ��Ȱ��ȭ
            minesound.enabled = false; //���嵵 ��
            GameManager_Proto.gm.PlayerDead();//�÷��̾� ��� ����
        }
        else
        {
            Debug.LogWarning("mine :: �νĵ� Rigidbidy ����!");
        }
    }

    public void Offlight()
    {
        lights.SetActive(false);
    }

}


