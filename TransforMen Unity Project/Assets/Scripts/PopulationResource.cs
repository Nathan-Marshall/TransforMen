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

    public PopulationResource(int quantity)
    {
        SetResourcePopulation();
        SetQuantity(quantity);
        SetCanBeHarvested(true);
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
