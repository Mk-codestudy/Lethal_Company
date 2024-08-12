using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPod : MonoBehaviour // droppod �� dropstart ����Ʈ���� dropendpoint���� ���ϸ� lerp�� ���� ����
{

    public Transform startPoint;
    public Transform endPoint;
    public float totalDropDuration = 7f; // �� ������ �ð� 
    public float speed; // �ʱ� �ӵ�
    public float easing; // ���� ����
    public GameObject shover;

    public GameObject item1; // pod���� ������ ������
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;

    public Transform regenPos1;
    public Transform regenPos2;
    public Transform regenPos3;
    public Transform regenPos4;
    private bool hasDropped = false; // ���� ���θ� Ȯ��


    private float startTime; // ���� ���� �ð�
    private Vector3 velocity;      // ���� �ӵ�

    void Start()
    {
        transform.position = startPoint.position; // �ʱ� ��ġ�� startpoint�� ��ġ

        startTime = Time.time; // ���ۺ��� ��������� �ð��� ���

                               // Time.time ���� �ð�

        velocity = (endPoint.position - startPoint.position) * speed * Time.deltaTime;

        Rigidbody rb1 = item1.GetComponent<Rigidbody>();
        Rigidbody rb2 = item2.GetComponent<Rigidbody>();
        Rigidbody rb3 = item3.GetComponent<Rigidbody>();
        Rigidbody rb4 = item4.GetComponent<Rigidbody>();

       

    }

    void Update()
    {
        float elapsedTime = (Time.time - startTime) / totalDropDuration; // ����ð��� ���

        // ���ϰ� �������� elapsedTime�� 1�� ����
        if (elapsedTime > 1f)
        {
            elapsedTime = 1f;
        }

        float easedTime = Mathf.Pow(elapsedTime, easing); // �󸶳� ���� ���� ���
        float currentSpeed = Mathf.Lerp(speed, 0, elapsedTime); // ������ ���� ���� ���� ��ŭ ���� �ӵ����� 0���� �����Ѵ�.

        velocity = Vector3.down * currentSpeed;  // �ӵ� ���͸� ���� �ӵ��� ���        
        Vector3 displacement = velocity * Time.deltaTime;  // ������ ���� �ӵ� * Time.deltatime �� ���Ͽ� deltatime ���� �̵��� �Ÿ��� ����        
        transform.position += displacement; // ��ġ ������Ʈ

        if(transform.position.y <= endPoint.position.y) // transform �� y �ప�� ���������� y������ ũ�ų� ������������ �����ߴ����� Ȯ��
        {
            transform.position = endPoint.position; 


            if (!hasDropped)
            {
                hasDropped = true; // ���� �� �� ���� ����ǵ��� ����

                Invoke("InstantiateItem", 1f); // 1�� �� shover �ν��Ͻ�ȭ
            }

        }
        
    }

    public void InstantiateItem() // �����۵����� ������ ��ġ�� ����
    {
        Instantiate(item1, regenPos1.position, regenPos1.rotation); // �������� �� ���� ��ҿ��� ����
        Instantiate(item2, regenPos2.position, regenPos2.rotation); // 
        Instantiate(item3, regenPos3.position, regenPos3.rotation); // 
        Instantiate(item4, regenPos4.position, regenPos4.rotation); //
    }
}
