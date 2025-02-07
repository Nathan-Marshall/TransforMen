﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Ruins Unit: static unit which can be scavanged by human 
//    infantry to acquire people and scrap resources. Can NOT 
//    be influenced by enemies FOR NOW (subject to change) 
//-------------------------------------------------------------

public class Ruin : StaticUnit
{
    //Collaborators: Human Infantry 

    private string desc = "A ruined city. Can be scavanged by human infantry in search for scrap and survivors";

    int pop;
    int scrap;

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(false);
        SetDescription(desc);

        //TO UPDATE IN FUTURE:
        // the survivor and scrap count should be set in a more dynamic way
        // currently set statically for 1st prototype. 
        pop = 5;
        scrap = 5;

        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.Ruin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPopulation() { return pop; }
    public int GetScrap() { return scrap; }
}
