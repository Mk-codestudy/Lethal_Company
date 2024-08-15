using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitThumper : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager_Proto.gm.PlayerHit();
        }
    }
}
