using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap_3 : MonoBehaviour
{ // ������� ���� �ϴ� ��ũ��

    public float minValue = 60;
    public float maxValue = 120;
    public float minWeight = 32;
    public float maxWeight = 40;

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
