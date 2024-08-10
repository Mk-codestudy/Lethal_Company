using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveChildTag : MonoBehaviour
{
    void Start()
    {
        // 부모의 Tag를 자식들에게 할당
        foreach (Transform child in transform)
        {
            child.gameObject.tag = this.tag;
        }
    }

}
