using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Crawler Host: Dynamic entities spawned by crawler host. Will
//    stay near host but are not controlled by player. They 
//    can attack and be attacked 
//-------------------------------------------------------------

public class CrawlerSpawn : AttackUnit
{
    //Collaborators: Unit, Dynamic Unit, Attack Target, Attacker, Crawler Host 

    //AI-Control
    //Stays near crawler host 

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
