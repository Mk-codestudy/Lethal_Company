using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap_1 : MonoBehaviour
{
    // 볼트나 소형 스크랩
    public string itemName;
    //public Sprite itemIcon;
    public int itemID;


    public float minValue = 50;
    public float maxValue = 100;
    public float minWeight = 20;
    public float maxWeight = 30;

    private float value;
    private float weight;

    void Start()
    {
        value = Random.Range(minValue, maxValue);
        weight = Random.Range(minWeight, maxWeight);
        
    }

    void Update()
    {
       


    }

         
    




}
