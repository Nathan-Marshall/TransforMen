using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Population Resource: Can be harvested by human infantry
//    from ruins. Used with scrap metal to get more human 
//    infantry 
//-------------------------------------------------------------

public class PopulationResource : Resource
{
    private string desc = "Human survivors";

    // Start is called before the first frame update
    void Start()
    {
        SetResourcePopulation();
        //TO BE UPDATED FOR FUTURE PROTOTYPE: quantity of resource should be dynamically determined 
        SetQuantity(5);
        SetCanBeHarvested(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
