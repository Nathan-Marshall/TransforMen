using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Resistance Base: A static unit. Aliens win if this destroyed
//-------------------------------------------------------------

public class ResistanceBase : StaticUnit
{
    //Collaborators: Static Unit, Attack Target 
    private string desc = "Headquarters of the human resistance";
    //Starts spawned on the map 

    //Alien Win Condition 

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(true);
        SetDescription(desc);
        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.HQ);
    }
}
