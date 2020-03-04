using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Spike Resource: Dropped by spike alien, used to upgrade 
//    human infantry to spike dude 
//-------------------------------------------------------------

public class SpikeResource : Resource
{
    private string desc = "Dropped by spike alien";

    // Start is called before the first frame update
    void Start()
    {
        SetResourceSpike();
        //TO BE UPDATED FOR FUTURE PROTOTYPE: quantity of resource should be dynamically determined 
        SetQuantity(5);
        SetCanBeHarvested(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
