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


    //1. �÷��̾� HP ���� ~ ���
    [Header("�÷��̾� �������ͽ� ����")]
    public float playerHP = 100;
    public float playerSP = 100;

    [Header("�� ������")]
    public float damage = 15;

    //2. �� HP ���� ~ ���
    //���� �����̶� �ϳ��� ����� �ȵ����� ������Ÿ�Կ��� ������ �� �� HP�ִ� ���� ���� ��
    [Header("�� �������ͽ�")]
    public float dumperHP = 100;
    public float enumDamage = 30; //�� �ѹ� �����Ҷ����� �Դ� ��������

    [Header("��� UI ���� �ۺ� ����")]
    public GameObject deadUI;

    [Header("�÷��̾� �ǰ� UI �÷� �ڷ�ƾ")]
    public HitUICorutine hitUICorutine;



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

    }

    void Update()
    {
        //�÷��̾� ��� ���� 
        if (playerHP <= 0)
        {
            PlayerDead();
        }

        //�� �ѱ��
        if (Input.GetKeyDown(KeyCode.F7))            // F4 ������...
        {
            SceneLethal(-1);                        // ���� �� ��ŸƮ
        }
        if (Input.GetKeyDown(KeyCode.F5))            // F5 ������...
        {
            SceneLethal(0);                         // ���� �� ��ŸƮ
        }
        else if (Input.GetKeyDown(KeyCode.F8))       // F6 ������...
        {
            SceneLethal(1);                         // ���� �� ��ŸƮ
        }



    }


    //�÷��̾� ��� �Լ�
    public void PlayerDead()
    {
        //�÷��̾� ���׵� ����
        //���� �÷��̾� ���(�̵�, �׷�, ��Ÿ���...) ���
        //ī�޶� 3��Ī���� ��ȯ

        //UI����
        PlayerDeadUI();
    }
    public void PlayerDeadUI()
    {
        //Albedo 60������ ���� â ��濡 �ؽ�Ʈ
        //������:  UI�� �߰� �ϱ�

        //UIâ ȭ�� �����Ÿ��� "-------------" �� �� �Ʒ��� ��鸮�ٰ� > ����
        //[���� ��ȣ: ��������] �ؽ�Ʈ �۾����ٰ� ȭ�� ��ü�� ä�쵵�� Ŀ�� > ����

        //UIâ �ѱ�
        deadUI.SetActive(true);
        Text uitext = deadUI.transform.GetChild(1).GetComponent<Text>();


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
    public void PlayerHit(float EnumHP)
    {
        EnumHP -= 15; //������ ���ϱ�

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
