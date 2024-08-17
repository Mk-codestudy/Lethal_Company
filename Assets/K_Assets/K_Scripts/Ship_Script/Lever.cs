using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    [Header("�÷��̾� ���ӿ�����Ʈ")]
    public Transform player;

    [Header("���� ���� ��ġ transform")]
    public Transform leverGetPoint;

    //�÷��̾�� �� ���� �Ÿ� ���
    [Header("�÷��̾� ��ȣ�ۿ� ���� �Ÿ�")]
    [Range(1.0f, 6.0f)]
    public float interactionDistance = 2.0f; //��ȣ�ۿ� ������ �Ÿ�
    public bool showGizmoSphare;
    public bool showGizmoLine;

    //EŰ Ȧ�� �ð�
    [Header("E Ȧ�� �ð�")]
    [Range(0.5f, 3.0f)]
    public float doorHoldTime = 1.5f;
    public float currentHoldTime = 0;

    //UI
    [Header("��ô�� �����̴� UI")]
    public Slider progressSlider; // ��ô���� ǥ���� Slider

    [Header("E : �̷��ϱ� �ȳ� UI")]
    public GameObject holdText; //�ȳ� �ؽ�Ʈ

    [Header("�Լ� ����� ī�޶���ŷ:����ī�޶� CaneraShake�Ҵ�")]
    public CameraShake camshake;

    [Header("������ ����� n�� ������ �Ѿ��")]
    public int sceneNumber;
    public AudioClip[] audioClips;

    public AudioSource audioSource;

    [Header("���潺 �ʿ��� ���� ���� �̷��ϱ�")]
    public ShipMoving shipMoving;
    //public ShipDoor shipDoor;
    public float delaytodeparttime = 0.5f;
    public float closedoortime = 5f;
    public bool leverActivated;

    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Door :: Player ���ӿ�����Ʈ�� ������ �Ҵ����� ����!");
        }
        //player = GameObject.FindGameObjectWithTag("Player").transform; //ĳ��~

        if (progressSlider != null)
        {
            progressSlider.maxValue = doorHoldTime;
            progressSlider.value = 0f;
            progressSlider.gameObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        }
        holdText.SetActive(false);
        
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //���� �÷��̾��� �Ÿ��� ���.
        float distanceToPlayer = Vector3.Distance(leverGetPoint.transform.position, player.position);
        
        if (leverActivated)
        {
            LetsDePart();
        }

        if (distanceToPlayer > interactionDistance)
        {
            holdText.SetActive(false);
            return;
        }
        else
        {
            holdText.SetActive(true); //�ȳ� �ؽ�Ʈ �ѱ�

            if (Input.GetKey(KeyCode.E)) //�� �տ��� eŰ�� �� ���� ä ����ϱ�.
            {
                currentHoldTime += Time.deltaTime; //�󸶳� ������ �ֳ� �ð� ����...

                if (progressSlider != null)
                {
                    progressSlider.value = currentHoldTime; // ���൵�� Slider�� �ݿ�
                    progressSlider.gameObject.SetActive(true); // Slider Ȱ��ȭ
                }

                if (currentHoldTime > doorHoldTime) //���� ���� �ð��� �Ѿ��!
                {
                    //�Լ��� ��鸮�� ������ �Ͼ��.
                    //�Լ� ����������
                    audioSource.clip = audioClips[0];
                    audioSource.Play();
                    

                    //�� �Ѿ��
                    // ���� �� �ε���(����)�� Ȯ���� ��
                    int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
                    if (currentSceneindex == 1) //���� �����?
                    {
                        //�κ�ũ�� ���� ����
                        Invoke("TurnonSound", 0.5f);

                        //�κ�ũ�� 3.5�� �ڿ� ���Ѿ��
                        Invoke("LoadScene", 3.5f);
                        currentHoldTime = 0f;
                    }
                    else if (currentSceneindex == 2) //���潺 ���̶��?
                    {
                        leverActivated = true;
                    }
                }
            }
            else
            {
                currentHoldTime = 0;
                SliderReset();
            }
        }



    }

    private void LetsDePart()
    {
        if (delaytodeparttime > 0f)
        {
            delaytodeparttime -= Time.deltaTime;
        }
        else
        {
            shipMoving.departing = true; //�̷� ���ް�
        }

        if (closedoortime > 0f)
        {
            closedoortime -= Time.deltaTime;

        }
        else if (closedoortime <= 0f)
        {
            GameManager_Proto.gm.AfterDepart();
        }
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
            Debug.Log("�����̴��� �� ����!");
        }
    }

    public void LoadScene()
    {

        if (sceneNumber == 1) //Ÿ�̸� ���� ���� �ڵ�
        {
            GameObject clock = GameObject.Find("MapUI");

            if (clock != null)
            {
                clock.SetActive(false);
            }
            else
            {
                Debug.Log("�ð� �� ã����!");
            }
        }

        SceneManager.LoadScene(sceneNumber); //�̰� ��¥ �ε��
    }

    void TurnonSound()
    {
        camshake.letsShake = true;
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }



    #region ������ �Ÿ� �׸���
    private void OnDrawGizmos()
    {
        // Gizmos ���� ����
        Gizmos.color = Color.green;

        if (showGizmoLine)
        {
            Gizmos.color = Color.yellow;
            // ���� �÷��̾� ������ �� �׸���
            Gizmos.DrawLine(leverGetPoint.transform.position, player.position);
        }

        if (showGizmoSphare)
        {
            Gizmos.color = Color.green;
            // ��ȣ�ۿ� ������ �Ÿ� ǥ��
            Gizmos.DrawWireSphere(leverGetPoint.transform.position, interactionDistance);
        }
    }
    #endregion

}
