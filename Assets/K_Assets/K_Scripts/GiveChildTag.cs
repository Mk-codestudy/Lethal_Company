using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveChildTag : MonoBehaviour
{
    void Start()
    {
        // �θ��� Tag�� �ڽĵ鿡�� �Ҵ�
        foreach (Transform child in transform)
        {
            child.gameObject.tag = this.tag;
        }
    }

}
