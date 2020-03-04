using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Transport Truck: vehicle capable of transporting human 
//    dynamic units 
//-------------------------------------------------------------

public class TransportTruck : Vehicle
{
    //Collaborators: Dynamic Unit, Attack Target, Vehicle 
    private string desc = "Truck for transporting resistance soldiers ";
    //CURRENT COST: 5 scrap

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
