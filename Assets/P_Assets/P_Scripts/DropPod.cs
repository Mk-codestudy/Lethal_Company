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
   

    public GameObject item1; // pod���� ������ ������
    public GameObject item2;
    

    public Transform regenPos1;
    public Transform regenPos2;
    
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

        }
        
    }

    private void OnTriggerEnter(Collider other) //������ �ٵ� �� �ֱ�...
    {
     
        if (other.CompareTag("ItemDrop"))
        {
            

            if (!hasDropped)
            {
                Debug.Log("��ҳ�?");
                hasDropped = true;
                Invoke("InstantiateItem", 4f);
            }
        
        }
                    
    }

    

    public void InstantiateItem() // �����۵����� ������ ��ġ�� ����
    {
        if(item1 != null && item2 != null)
        {
            Instantiate(item1, regenPos1.position, regenPos1.rotation); // �������� �� ���� ��ҿ��� ����
            Instantiate(item2, regenPos2.position, regenPos2.rotation); // 
        }
        
    }
       
    
}
