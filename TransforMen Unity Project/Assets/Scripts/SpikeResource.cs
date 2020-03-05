﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Spike Resource: Dropped by spike alien, used to upgrade 
//    human infantry to spike dude 
//-------------------------------------------------------------

public class SpikeResource : Resource
{
    private string desc = "Dropped by spike alien";

    public SpikeResource(int quantity)
    {
        SetResourceSpike();
        //TO BE UPDATED FOR FUTURE PROTOTYPE: quantity of resource should be dynamically determined 
        SetQuantity(quantity);
        SetCanBeHarvested(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
