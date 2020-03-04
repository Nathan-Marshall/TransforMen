using System.Collections;
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

    List<Resource> resources = new List<Resource>(); 

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(false);
        SetDescription(desc);

        //TO UPDATE IN FUTURE:
        // the survivor and scrap count should be set in a more dynamic way
        // currently set statically for 1st prototype. 
        resources.Add(new PopulationResource());
        resources.Add(new ScrapResource());

        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.RUIN);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
