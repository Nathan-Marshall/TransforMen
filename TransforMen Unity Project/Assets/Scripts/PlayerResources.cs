using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Player Resources: Keeps track of all the resources the player
//    currently has 
//-------------------------------------------------------------

public class PlayerResources : MonoBehaviour
{
    //private string desc = "Scrap metal that can be used for many purposes";
    //private string desc = "Dropped by spike alien";
    //private string desc = "Human survivors";
    //private string desc = "Dropped by alien crawler";

    int crawlbits;
    int population;
    int scrap;
    int spikes;

    // Start is called before the first frame update
    void Start()
    {
        crawlbits = 0;
        population = 50;
        scrap = 50;
        spikes = 5;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Crawlbits
    public void AddCrawlbits(int toAdd) {
        if (toAdd >= 0) {
            crawlbits += toAdd;
        } else {
            Debug.LogError("Failed to add "+toAdd+" crawlbits.");
        }
    }
    public bool SpendCrawlbits(int cost) {
        if (cost >= 0) {
            if (crawlbits < cost) {
                return false;
            } else {
                crawlbits -= cost;
                return true;
            }
        } else {
            Debug.LogError("Failed to spend " + cost + " crawlbits.");
            return false;
        }
    }
    public int GetCrawlbitResource() { return crawlbits; }

    //Population
    public void AddPopulation(int toAdd) {
        if (toAdd >= 0) {
            population += toAdd;
        }
        else {
            Debug.LogError("Failed to add " + toAdd + " population.");
        }
    }
    public bool SpendPopulation(int cost) {
        if (cost >= 0) {
            if (population < cost) {
                return false;
            }
            else {
                population -= cost;
                return true;
            }
        }
        else {
            Debug.LogError("Failed to spend " + cost + " population.");
            return false;
        }
    }
    public int GetPopulationResource() { return population; }

    //Scrap
    public void AddScrap(int toAdd) {
        if (toAdd >= 0) {
            scrap += toAdd;
        }
        else {
            Debug.LogError("Failed to add " + toAdd + " scrap.");
        }
    }
    public bool SpendScrap(int cost) {
        if (cost >= 0) {
            if (scrap < cost) {
                return false;
            }
            else {
                scrap -= cost;
                return true;
            }
        }
        else {
            Debug.LogError("Failed to spend " + cost + " scrap.");
            return false;
        }
    }
    public int GetScrapResource() { return scrap; }

    //Spikes 
    public void AddSpikes(int toAdd) {
        if (toAdd >= 0) {
            spikes += toAdd;
        }
        else {
            Debug.LogError("Failed to add " + toAdd + " spikes.");
        }
    }
    public bool SpendSpikes(int cost) {
        if (spikes >= 0) {
            if (spikes < cost) {
                return false;
            }
            else {
                spikes -= cost;
                return true;
            }
        }
        else {
            Debug.LogError("Failed to spend " + cost + " spikes.");
            return false;
        }
    }
    public int GetSpikeResource() { return spikes; }

}
