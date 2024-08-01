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

    bool soundOn = false;


    void Update()
    {
        //ringTime���� �ߺ� �Ҹ��� ����.
        if (currenttime < ringTime)
        {
            currenttime += Time.deltaTime;
        }
        else
        {
            //����� ���
            currenttime = 0;
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        //print("CollisonEnter!"); //����!
        if (collision.gameObject.name.Contains("Player")) //������ ���ӿ�����Ʈ �̸��� �÷��̾ �ִ�?
        {
            rb = collision.gameObject.GetComponent<Rigidbody>(); //ã�� ������Ʈ ������ٵ� ��������...
            print("Debug_������ٵ� ���� :" + rb.gameObject.name);

            Invoke("Explosion", 0.5f);
        }         
    }

    public void Explosion()
    {
        if (rb != null) //������...
        {
            rb.AddExplosionForce(800, transform.position, radius, 60); //(���� �Ŀ�, ������ �߽���ġ, ����, ���� Ƣ����� �Ÿ���)
                                                                       //�÷��̾� ��� ����
                                                                       //��ƼŬ ��
                                                                       //���ڴ� �����

            Destroy(gameObject);
        }
        else
        {
            print("�νĵ� Rigidbidy ����!");
        }
    }
}


