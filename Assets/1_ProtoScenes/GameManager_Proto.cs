using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_Proto : MonoBehaviour
{
    static public GameManager_Proto gm;
    //prototype�� ���� ���ӸŴ���.
    //���� ��Ÿ�� �Ѿ������ �ʿ��� �Լ��� �����ϱ�.

    #region protype���� �����Ǿ�� �� ����
    //�÷��̾� HP
    //�� HP
    //��� ���� ��ü�� ���
    //�÷��̾� ��� ���� (��� UI ����̶�)

    //�� �ѱ�� (F7, F8)
    #endregion

    #region alpha���� �����Ǿ�� �� ���� 

    //�÷��̾ ������ �и�
    //���Ͱ� �±�

    #endregion

    //1. �÷��̾� HP ���� ~ ���
    [Header("�÷��̾� �������ͽ� ����")]
    public GameObject player;
    public float playerHP = 100;
    public float playerSP = 100;

    [Header("�� ������")]
    public float damage = 15;

    //2. �� HP ���� ~ ���
    //���� �����̶� �ϳ��� ����� �ȵ����� ������Ÿ�Կ��� ������ �� �� HP�ִ� ���� ���� ��
    [Header("�� �������ͽ�")]
    public Thumper thumper;
    bool isthumpAlive = true;
    public float dumperHP = 100;
    public float enumDamage = 30; //�� �ѹ� �����Ҷ����� �Դ� ��������

    [Header("��� UI ���� �ۺ� ����")]
    public GameObject deadUI; //��ü��ȣ ��������
    public EnterFactory enterFactory; //�༺�̶� ���� �����ϴ� Ŭ����
    public Transform closeDoorCamPos; //�� �����°� ������ ī�޶� ��ġ
    public bool isCenemaStart; //�ó׸�ƽ �ѹ��� �Ѱ� ����
    public ShipMoving shipMoving; //�Լ� ���׽�ų Ŭ����
    public GameObject checkUI;
    public GameObject decased;

    [Header("��� �� ������ �ð�")]
    public float deadAfterDelay = 1.5f;
    public float currenttime = 0;

    [Header("�÷��̾� �ǰ� UI �÷� �ڷ�ƾ")]
    public HitUICorutine hitUICorutine;

    AudioSource audioSource;
    bool alreadyPlayed; //���� �ѹ��� ����ϰ����ִ� �ڵ�
    bool dieCinemaPlayed; //�Լ� �̷��ϴ� ���׸ӽ��� ��������

    [Header("������� ���� ���ӿ�����")]
    public GameObject backgroundMusic;

    [Header("�Լ� �� ����")]
    public ShipDoor shipDoor;
    public YouMustOpenDoor mustopendoor;

    [Header("UI����")]
    public GameObject mapUI;
    public GameObject playerstate;
    public GameObject resultUI;
    public float printresultTime = 2.5f;
    public GameObject endpointCam;
    
    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        //��� UI ���� �ʰ� ���α�
        deadUI.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //�÷��̾� ��� ���� 
        if (playerHP <= 0)
        {
            PlayerDead();
        }

        #region ������Ÿ�� ��� - �� �ѱ��
        //�� �ѱ��
        //if (Input.GetKeyDown(KeyCode.F7))            // F4 ������...
        //{
        //    SceneLethal(-1);                        // ���� �� ��ŸƮ
        //}
        //if (Input.GetKeyDown(KeyCode.F5))            // F5 ������...
        //{
        //    SceneLethal(0);                         // ���� �� ��ŸƮ
        //}
        //else if (Input.GetKeyDown(KeyCode.F8))       // F6 ������...
        //{
        //    SceneLethal(1);                         // ���� �� ��ŸƮ
        //}
        #endregion


    }


    //�÷��̾� ��� �Լ�
    public void PlayerDead()
    {
        //�÷��̾� ���׵� ����
        //���� �÷��̾� ���(�̵�, �׷�, ��Ÿ���...) ���

        //ī�޶� 3��Ī���� ��ȯ

        //UI����
        PlayerDeadUI();

        //�ð� ����
        mapUI.SetActive(false);
        //�÷��̾� ����UI ����
        playerstate.SetActive(false);
        //��Ʈ �ڷ�ƾ ����
        hitUICorutine.gameObject.SetActive(false);

        if (player.GetComponent<CharacterController>() != null) //���� ������ null��
        {
            //�״� �Ҹ�
            if (!alreadyPlayed)
            {
                audioSource.Play();
                alreadyPlayed = true;
            }
        }

        //1~2������ ��ٸ�...
        if (currenttime <= deadAfterDelay)
        {
            currenttime += Time.deltaTime;
        }
        else
        {
            deadUI.SetActive(false);
            //�Լ��� �����Ϸ��� offens���� Ȱ��ȭ�Ǿ�� �Ѵ�.
            enterFactory.GotoAnotherRoom(true);
            player.SetActive(false);//�÷��̾� ���� (ī�޶� ��� ����)

            //�뷡 ��.
            backgroundMusic.SetActive(false);

            if (!isCenemaStart)
            {
                //�Լ� ���׽���.
                shipMoving.departing = true;
                //���׸ӽ� �� �ѹ��� Ʋd��.
                PlayerDieCimena.instance.StartCinemachime();
                isCenemaStart = true;
            }
            else
            {
                if (PlayerDieCimena.instance.isCinemaEnd == true)//���׸ӽ��� ������...
                {
                    AfterDepart(); //��� â.
                }
            }
        }
    }


    public void PlayerDeadUI()
    {
        //Albedo 60������ ���� â ��濡 �ؽ�Ʈ
        //������:  UI�� �߰� �ϱ�

        //UIâ ȭ�� �����Ÿ��� "-------------" �� �� �Ʒ��� ��鸮�ٰ� > ����
        //[���� ��ȣ: ��������] �ؽ�Ʈ �۾����ٰ� ȭ�� ��ü�� ä�쵵�� Ŀ�� > ����

        //UIâ �ѱ�
        deadUI.SetActive(true);
        //Text uitext = deadUI.transform.GetChild(1).GetComponent<Text>();


        #region �ڷ�ƾ���� ��Ʈ ������ �ø��� > ���Ŀ��� ���
        //UI��Ʈ = 18�� �����ؼ� 170���� �ø���!!!
        //print(uitext.fontSize);

        //float percent = 0; // ��Ʈ ���� ����
        //percent += Time.deltaTime;

        //float size = Mathf.Lerp(18, 170, percent); //Lerp�� ��� �÷��ַ��� �ߴµ�...
        //uitext.fontSize = Mathf.CeilToInt(size); //�ѹ��� ����Ǵ°Ͱ��Ҵ�.
        #endregion
    }


    //�÷��̾ ������ �Լ�
    //�÷��̾ ������ ������ ��ũ��Ʈ�� gm.playerhit(dumperHP) ���� ������ ���
    public void PlayerHit()
    {
        if (isthumpAlive)
        {
            dumperHP -= damage; //������ ���ϱ�
            if (dumperHP > 0)
            {
                thumper.Damaged();
            }
            else
            {
                thumper.Dead();
                isthumpAlive = false;
            }
        }

    }

    public void PlayerOnDamaged()
    {
        //ī�޶� ������ ���� ����
        //���տ� ���̴� �÷��̾� �𵨸� ���� ������

        //������Ÿ���� ��� UI�ڷ�ƾ �ֱ�
        StartCoroutine(hitUICorutine.FadeOut());
    }

    //���� ������ �Լ�
    public void AnemHit()
    {
         playerHP -= enumDamage; //�� ��������ŭ �÷��̾� HP ����
    }


    public void AfterDepart()
    {
        if (Camera.main == null)
        {
            endpointCam.GetComponent<AudioListener>().enabled = true;
        }
        shipDoor.isDoorOpen = false;
        mustopendoor.timer = 1000;
        if (printresultTime > 0)
        {
            printresultTime -= Time.deltaTime;
        }
        else
        {
            //�ð� ����
            mapUI.SetActive(false);
            //�÷��̾� ����UI ����
            playerstate.SetActive(false);
            //�뷡 ��.
            backgroundMusic.SetActive(false);
            //�ð� ����. 
            Time.timeScale = 0;

            //��� UI ���.
            resultUI.SetActive(true);

            //�÷��̾� ���/�ƴ� ���ο� ���� üũ �ٲٱ�
            if (playerHP > 0)
            {
                checkUI.SetActive(true);
                decased.SetActive(false);
            }
            else
            {
                decased.SetActive(true);
                checkUI.SetActive(false);
            }

        }
    }


    //���� �������� �� ������ �Լ�
    public void RoundOver(int num)
    {
        SceneManager.LoadScene(num);

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

    

    #region �� ���� �Լ� 

    void SceneLethal(int num)
    {
        // ���� �� �ε���(����)�� Ȯ���� ��
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;

        // F4 ~ F6�� ���� �ش� �� �ҷ�����
        // F4(����), F5(����), F6(����)
        SceneManager.LoadScene(currentSceneindex + num);

        // Ŀ�� ���󺹱� (���� �ش� ������ ������ �� Ŀ�� �Ⱥ��̰� ������ ������ �״�� �ݿ���)
        Cursor.lockState = CursorLockMode.Confined;
    }

    #endregion

}
