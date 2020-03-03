using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Melee: a weapon (physical attack) used by attack unit
//-------------------------------------------------------------

public class Melee : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        SetRange(5);
        SetSpeed(25);
        SetDamage(28);
        SetFiringRate(6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
