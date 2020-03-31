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
    //Consumes resources 

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(true);
        SetDescription(desc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
