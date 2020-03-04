using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Garage: creates transport trucks and consumes resources. 
//    can be attacked and destroyed by enemy 
//-------------------------------------------------------------

public class Garage : StaticUnit
{
    //Collaborators: Static Unit, Attack Target 
    private string desc = "Garage where trucks are built";
    //COST: 5 scrap 

    //Can create trucks
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
