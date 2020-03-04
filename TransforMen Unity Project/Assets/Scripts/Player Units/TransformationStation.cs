using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Transformation Station: capable of upgrading upgradeable
//     dynamic units 
//-------------------------------------------------------------

public class TransformationStation : StaticUnit
{
    // Collaborators: Static Unit, Attack Target 
    private string desc = "Station where the most dedicated soldiers are evolved into something greater";
    //CURRENT COST: 5 scrap

    //Can upgrade/transform units 
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
