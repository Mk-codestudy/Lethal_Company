using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Kettle : Item // // Monobehaviour -> Item - > Kettle : item ���� ��ӹ���
{
    //public Vector3 itemScale = new Vector3(0.06f, 0.06f, 0.06f); // �����ڰ� ������ Ŀ�� �������� ���̸� ĳ���Ϳ� �̵��ϴµ� ������ ���ܼ� ��ũ��Ʈ������ ����




    private void Awake()
    {
        itemName = "������";
        itemValue = Random.Range(100, 200);
    }

        public override void Start()
    {
        base.Start(); // �θ��� start �Լ��� ȣ���ؼ� ��

       // Transform currentTransform = transform; // ���� �� �������� transform, ������Ʈ�� ������
        //transform.localScale = itemScale; // ���� ������ ���� �׸��� �ν����ͻ󿡼� �������� 1�̳� �θ��� �����ϰ� �����ϰ� ��. �׷��� �̵��� ������ ���ټ�����



    }

    public override void Update()
    {
        base.Update(); // �θ��� update �Լ�- Bilboard�� �����ͼ� ����
    }

    public override void ShowItemInfo()
    {
        base.ShowItemInfo(); // �θ� �ִ� ShowItemInfo()  �Լ� ����
    }

    public override void HideItemInfo()
    {
        base.HideItemInfo(); // �θ� �ִ� HideItemInfo() �Լ� ����
    }






}





   