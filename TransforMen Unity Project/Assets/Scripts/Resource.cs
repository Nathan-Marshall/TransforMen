using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Resource: Can be harvested for a resource by human infantry
//-------------------------------------------------------------

public class Resource
{
    //Collaborators: Static Unit 

    //I feel like maybe this class doesn't currently make much sense? 
    public enum ResourceType { Population, Scrap, Spike, Crawlbit }; //kind of resources that can be found 
    protected ResourceType resource;
    protected int quantity;
    protected bool canBeHarvested; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get & Set ResourceType 
    public ResourceType GetResourceType() { return resource; }
    protected void SetResourcePopulation()
    {
        resource = ResourceType.Population;
    }
    protected void SetResourceScrap()
    {
        resource = ResourceType.Scrap;
    }
    protected void SetResourceSpike()
    {
        resource = ResourceType.Spike;
    }
    protected void SetResourceCrawlbit()
    {
        resource = ResourceType.Crawlbit;
    }

    //Get & Set Quantity 
    public int GetQuantity() { return quantity; }
    protected void SetQuantity(int newQuantity)
    {
        quantity = newQuantity;
    }

    //Increment and decrement quantity 
    public void AddResource(int toAdd)
    {
        quantity += toAdd; 
    }

    public void LoseResource(int toLose)
    {
        quantity -= toLose;

        if (quantity < 0)
        {
            quantity = 0; 
        }
    }

    //Get & Set CanBeHarvested
    public bool GetCanBeHarvested() { return canBeHarvested; }
    protected void SetCanBeHarvested(bool newHarvest)
    {
        canBeHarvested = newHarvest;
    }
}
