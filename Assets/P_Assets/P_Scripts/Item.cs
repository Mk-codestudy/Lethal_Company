using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour // �� ������ ��ũ��Ʈ�� �ٸ� ������ ��ũ��Ʈ�� ����Ѵ�.
{

    public string itemName;
    public int itemValue;

    public TMP_Text itemName_text; // ������ �̸�
    public TMP_Text itemName_value; // ������ ���� �� ǥ��
    public Sprite itemSprite; // �������� ��������Ʈ �̹��� 

    public Camera mainCamera; 

    public virtual void Start()          


    {
        mainCamera = Camera.main; // ������ ���� ī�޶� ã�Ƽ� �� ������ ��´�.


        // ������ ������ ���ÿ� ui ��Ȱ��ȭ
        if (itemName_text != null)
        {
            itemName_text.gameObject.SetActive(false);
        }
        if (itemName_value != null)
        {
            itemName_value.gameObject.SetActive(false);
        }
    }

    public virtual void Update() // 
    {
        if(itemName_text != null)
        {
            itemName_text.text = itemName; // ������ �̸�ǥ��
        }
        if(itemName_value != null)
        {
            itemName_value.text = itemValue.ToString(); // ��ġǥ��
        }
       
        



        //if (itemName_text != null && itemName_value != null)
        //{
        //    Vector3 dir1 = itemName_text.transform.position - Camera.main.transform.position;  // ����ī�޶�(player) �� �ٶ󺸴� ���� ����
        //    Vector3 dir2 = itemName_value.transform.position - Camera.main.transform.position; // ����ī�޶�(player) �� �ٶ󺸴� ���� ����

        //    Quaternion lookRotation1 = Quaternion.LookRotation(dir1); // ī�޶� ���� �������� �ٶ󺸴� rotation
        //    Quaternion lookRotation2 = Quaternion.LookRotation(dir2); // ī�޶� ���� �������� �ٶ󺸴� rotation

        //    itemName_text.transform.rotation = lookRotation1;
        //    itemName_value.transform.rotation = lookRotation2;



        //}
    }


    public virtual void ShowItemInfo() // �ڽĿ� ����ϱ����� virtual �� 
    {
        // ��Ŭ���ϸ� ����� ui ���̰� �ϱ�
        if(itemName_text != null)
        {
            itemName_text.gameObject.SetActive(true);
        }
        if(itemName_value)
        {
            itemName_value.gameObject.SetActive(true);
        }
             
        

        Invoke("HideItemInfo", 3f);  // Invoke�� ȣ���� �� �޼��� �̸��� ���ڿ��� ���� �� �� �ִ�. 3�� �� ui �� ����.
    }


    // 3�� �ڿ� ui �ٽ� �����
    public virtual  void HideItemInfo()
    {
        if(itemName_text != null)
        {
            itemName_text.gameObject.SetActive(false);
        }
        if(itemName_value != null)
        {
            itemName_value.gameObject.SetActive(false);
        }
        
    }




}
