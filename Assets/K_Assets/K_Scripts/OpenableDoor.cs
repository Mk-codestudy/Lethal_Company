using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class OpenableDoor : MonoBehaviour
{
    //�ٽ� eŰ�� �� ������ ���� ����.

    //Ư�� ���͵��� ���� �� �� ����.

    public Animator animator;

    public Transform player;

    //�÷��̾�� �� ���� �Ÿ� ���
    [Range(1.0f, 6.0f)]
    public float interactionDistance = 2.0f; //��ȣ�ۿ� ������ �Ÿ�

    #region ������ �Ÿ� Ȯ���ϱ�
    public Color gizmoColor = Color.green;
    #endregion

    //EŰ Ȧ�� �ð�
    public float doorHoldTime = 1.5f;
    public float currentHoldTime = 0;

    //�� ������ ���� Ȯ��
    bool isDoorOpen = false;

    //UI
    public Slider progressSlider; // ��ô���� ǥ���� Slider

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; //ĳ��~

        // Slider�� �ʱ�ȭ
        if (progressSlider != null)
        {
            progressSlider.maxValue = doorHoldTime;
            progressSlider.value = 0f;
            progressSlider.gameObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        }

    }

    void Update()
    {
        //���� �÷��̾��� �Ÿ��� ���.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer > interactionDistance)
        {
            return;
        }
        else if (Input.GetKey(KeyCode.E)) //�� �տ��� eŰ�� �� ���� ä ����ϸ� ���� ����.
        {
            currentHoldTime += Time.deltaTime; //�󸶳� ������ �ֳ� �ð� ����...

            if (progressSlider != null)
            {
                progressSlider.value = currentHoldTime; // ���൵�� Slider�� �ݿ�
                progressSlider.gameObject.SetActive(true); // Slider Ȱ��ȭ
            }

            if (currentHoldTime > doorHoldTime) //���� ���� �ð��� �Ѿ��!
            {
                //���� �������� ���ȴ��Ŀ� ���� ���� ������ ����

                if (!isDoorOpen)
                {
                    OpenTheDoor();
                    //���� ���
                    currentHoldTime = 0;
                    SliderReset();

                }
                else if (isDoorOpen)
                {
                    CloseTheDoor();
                    //���� ���
                    currentHoldTime = 0;
                    SliderReset();
                }

            }
        }
        else
        {
            currentHoldTime = 0;
            SliderReset();
        }
    }

    public void OpenTheDoor()
    {
        //�� ����
        print("Door Open!");
        animator.SetTrigger("Openning");
        isDoorOpen = true;
    }

    public void CloseTheDoor()
    {
        //�� �ݱ�
        print("Door Closed!");
        animator.SetTrigger("Closing");
        isDoorOpen = false;
    }

    public void DestroyDoor()
    {
        print("Door Destroy!");
        //�� �ı� ���� ����
        gameObject.SetActive(false); //�� ��Ȱ��ȭ�ϱ�
    }

    void SliderReset()
    {
        if (progressSlider != null)
        {
            progressSlider.value = 0f;
            progressSlider.gameObject.SetActive(false); // �Ϸ� �� ��Ȱ��ȭ
        }
        else
        {
            print("�����̴��� �� ����!");
        }
    }

    #region ������ �Ÿ� �׸���
    private void OnDrawGizmos()
    {
        // Gizmos ���� ����
        Gizmos.color = gizmoColor;

        // ���� �÷��̾� ������ �� �׸���
        Gizmos.DrawLine(transform.position, player.position);

        // ��ȣ�ۿ� ������ �Ÿ� ǥ��
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
    #endregion

}
