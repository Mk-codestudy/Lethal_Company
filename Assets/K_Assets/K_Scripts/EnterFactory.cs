using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterFactory : MonoBehaviour
{
    [Header("n�� ������ ����!")]
    public int sceneNumber = 0;

    [Header("�÷��̾� ���ӿ�����Ʈ")]
    public Transform player;

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
    float currentHoldTime = 0;


    //UI
    [Header("��ô�� �����̴� UI")]
    public Slider progressSlider; // ��ô���� ǥ���� Slider

    [Header("UI ĵ���� ����")]
    public GameObject canvas;
    


    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Door :: Player ���ӿ�����Ʈ�� ������ �Ҵ����� ����!");
        }
        player = GameObject.FindGameObjectWithTag("Player").transform; //ĳ��~

        // Slider�� �ʱ�ȭ
        if (progressSlider != null)
        {
            progressSlider.maxValue = doorHoldTime;
            progressSlider.value = 0f;
            progressSlider.gameObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        }

        //mapManager.LoadMapState();


    }

    void Update()
    {
        //���� �÷��̾��� �Ÿ��� ���.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > interactionDistance)
        {
            canvas.SetActive(false); //UIȰ��ȭ
            return;
        }
        else if (Input.GetKey(KeyCode.E)) //�� �տ��� eŰ�� �� ���� ä ����ϸ� ���� ����.
        {
            canvas.SetActive(true);
            currentHoldTime += Time.deltaTime; //�󸶳� ������ �ֳ� �ð� ����...

            if (progressSlider != null)
            {
                progressSlider.value = currentHoldTime; // ���൵�� Slider�� �ݿ�
                progressSlider.gameObject.SetActive(true); // Slider Ȱ��ȭ
            }

            if (currentHoldTime > doorHoldTime) //���� ���� �ð��� �Ѿ��!
            {
                //�����ϱ�
                //mapManager.SaveMapState(); //�ȵǰٴ�.
                //�� �Ѿ��
                SceneManager.LoadScene(sceneNumber);
            }
        }
        else
        {
            currentHoldTime = 0;
            SliderReset();
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
            print("�����̴��� �� ����!");
        }
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
            Gizmos.DrawLine(transform.position, player.position);
        }

        if (showGizmoSphare)
        {
            Gizmos.color = Color.green;
            // ��ȣ�ۿ� ������ �Ÿ� ǥ��
            Gizmos.DrawWireSphere(transform.position, interactionDistance);
        }
    }
    #endregion


}
