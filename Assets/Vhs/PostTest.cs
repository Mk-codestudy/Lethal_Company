using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using VolFx;

public class PostTest : MonoBehaviour
{
    Volume postVolume;

    private void Start()
    {
        postVolume = GetComponent<Volume>();
        Invoke("DelayPost", 3.0f);
    }

    void DelayPost()
    {
        VhsVol vhs;
        postVolume.profile.TryGet<VhsVol>(out vhs);
        vhs.active = true;
    }
}
