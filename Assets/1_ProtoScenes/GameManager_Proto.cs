using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Proto : MonoBehaviour
{
    //prototype�� ���� ���ӸŴ���.
    //���� ��Ÿ�� �Ѿ������ �ʿ��� �Լ��� �����ϱ�.

    //protype���� �����Ǿ�� �� ����

    //1. �÷��̾� HP ���� ~ ���
    [Header("�÷��̾� �������ͽ� ����")]
    public float playerHP = 100;
    public float playerSP = 100;

    [Header("�� ������")]
    public float damage = 15;

    //2. �� HP ���� ~ ���
    //���� �����̶� �ϳ��� ����� �ȵ����� ������Ÿ�Կ��� ������ �� �� HP�ִ� ���� ���� ��
    public float dumperHP = 100;

    public float enumDamage = 30; //�� �ѹ� �����Ҷ����� �Դ� ��������



    static public GameManager_Proto gm;

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
        
    }

    void Update()
    {
        //�÷��̾� ��� ���� 
        if (playerHP <= 0)
        {
            PlayerDead();
        }
    }


    //�÷��̾� ��� �Լ�
    public void PlayerDead()
    {
        //�÷��̾� ���׵� ����
        //���� �÷��̾� ���(�̵�, �׷�, ��Ÿ���...) ���
        //ī�޶� 3��Ī���� ��ȯ
        //��ü ��ȣ ��Ȱ��ȭ UI
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
    }


    //���� ������ �Լ�
    public void AnemHit()
    {
         playerHP -= enumDamage; //�� ��������ŭ �÷��̾� HP ����
    }


}
