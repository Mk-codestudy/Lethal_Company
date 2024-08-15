using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumperActionScript : MonoBehaviour
{
    public AudioClip[] footSound;
    public AudioSource audioSource;

    public Thumper thumper;
    public void PlayFootSound()
    {
        audioSource.clip = footSound[UnityEngine.Random.Range(0, 3)];
        audioSource.Play();
    }

    public void Attack()
    {
        if (thumper.thpstate == Thumper.ThpState.AttackDelay || thumper.thpstate == Thumper.ThpState.Attack)
        {
            GameManager_Proto.gm.AnemHit();
            GameManager_Proto.gm.PlayerOnDamaged();
        }
    }
}
