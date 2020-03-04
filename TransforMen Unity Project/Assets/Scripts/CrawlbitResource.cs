using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Crawlbit Resource: Dropped by alien crawler. Can be used 
//    to upgrade human infantry to crawler host. 
//-------------------------------------------------------------

public class CrawlbitResource : Resource
{
    private string desc = "Dropped by alien crawler";

    // Start is called before the first frame update
    void Start()
    {
        SetResourceCrawlbit();
        //TO BE UPDATED FOR FUTURE PROTOTYPE: quantity of resource should be dynamically determined 
        SetQuantity(5);
        SetCanBeHarvested(false); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
