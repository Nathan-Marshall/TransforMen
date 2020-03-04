using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Scrap Resource: Can be harvested by human infantry from ruins
//   and used to build many buildings, trucks and weapons (for 
//   converting population to human infantry) 
//-------------------------------------------------------------

public class ScrapResource : Resource
{
    private string desc = "Scrap metal that can be used for many purposes";

    // Start is called before the first frame update
    void Start()
    {
        SetResourceScrap();
        //TO BE UPDATED FOR FUTURE PROTOTYPE: quantity of resource should be dynamically determined 
        SetQuantity(5);
        SetCanBeHarvested(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
