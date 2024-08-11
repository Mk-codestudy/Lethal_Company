using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Duck : Item // Monobehaviour -> Item - > Duck : item ���� ��ӹ���
{
    private void Awake()
    {
        itemName = "����";
        itemValue = Random.Range(70, 100);
    }
    public override void Start()
    {
        base.Start(); // �θ��� start �Լ��� ȣ���ؼ� ��
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

    
