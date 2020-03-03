using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Alien Mothership: A static unit. Player wins if this destroyed
//     Spawns alien units. 
//-------------------------------------------------------------

public class AlienMothership : StaticUnit
{
    //Collaborators: Static Unit, Attack Target 

    //Human win condition
    //can spawn enemies 

    // Start is called before the first frame update
    void Start()
    {
        SetToAlienTeam();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
