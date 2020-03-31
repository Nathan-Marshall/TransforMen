using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Training Camp: which can train human infantry, and consumes
//      population/human resource. Can be attacked and destroyed 
//      by enemies
//-------------------------------------------------------------

public class TrainingCamp : StaticUnit
{
    //Collaborators: Static Unit, Attack Target 
    private string desc = "Training camp to train average people to soldiers";
    //COST: 5 scrap

    //Can train infantry 
    public GameObject infantryPrefab; 
    //Consumes resources 
    public int populationCost;
    public int scrapCost;
    private PlayerResources resources;
    private Collider boxCollider; 

    public bool Affordable
    {
        get
        {
            return resources.GetPopulationResource() >= populationCost
                    && resources.GetScrapResource() >= scrapCost;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(true);
        SetDescription(desc);

        resources = GameObject.Find("Game Control").GetComponent<PlayerResources>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

        if (boxCollider.Raycast(ray, out RaycastHit hit, 10000.0f))
        { 
            //If the building is clicked and can afford, make a new unit 
            if (Input.GetMouseButtonDown(0) && Affordable)
            {
                //Spawn a guy
                MakeInfantry(); 
            }
        }
    }

    void MakeInfantry()
    {
        resources.SpendPopulation(populationCost);
        resources.SpendScrap(scrapCost);

        Transform spawnLocation = transform.Find("SpernPernt");
        Vector3 spawnVec = spawnLocation.position; 
        GameObject infantry = Instantiate(infantryPrefab, spawnVec, Quaternion.identity);
    }
}
