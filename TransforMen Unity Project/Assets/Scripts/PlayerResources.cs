using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Player Resources: Keeps track of all the resources the player
//    currently has 
//-------------------------------------------------------------

public class PlayerResources
{
    CrawlbitResource crawlbits;
    PopulationResource population;
    ScrapResource scrap;
    SpikeResource spikes; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Crawlbits
    public void AddCrawlbits(CrawlbitResource toAdd)
    {
        crawlbits.AddResource(toAdd.GetQuantity());
    }
    public void LoseCrawlbits(int toLose)
    {
        crawlbits.LoseResource(toLose);
    }

    //Population
    public void AddPopulation(PopulationResource toAdd)
    {
        population.AddResource(toAdd.GetQuantity());
    }
    public void LosePopulation(int toLose)
    {
        population.LoseResource(toLose);
    }

    //Scrap
    public void AddScrap(ScrapResource toAdd)
    {
        scrap.AddResource(toAdd.GetQuantity());
    }
    public void LoseScrap(int toLose)
    {
        scrap.LoseResource(toLose);
    }

    //Spikes 
    public void AddSpikes(SpikeResource toAdd)
    {
        spikes.AddResource(toAdd.GetQuantity());
    }
    public void LoseSpikes(int toLose)
    {
        spikes.AddResource(toLose);
    }

}
