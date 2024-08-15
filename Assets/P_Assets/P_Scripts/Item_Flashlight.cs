using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Flashlight : Item
{
    private Light flashlight;


    private void Awake()
    {
        itemName = "Flashlight";
        itemValue = Random.Range(100, 100);

       
    }




    public override void Start()
    {
        flashlight = GetComponentInChildren<Light>();

        flashlight.enabled = false;
    }

    public override void Update()
    {
        
    }

    public void LightOnOff() // ºÒÀ» ²¯´Ù ÄÖ´ÙÇÏ´Â ÇÔ¼ö.
    {
        
            if (flashlight != null)
            {
                flashlight.enabled = !flashlight.enabled; // ²¯´ÙÄÖ´Ù ÇÏ±â
            }
        
    }



}
