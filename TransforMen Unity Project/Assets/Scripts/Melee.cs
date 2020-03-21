using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Melee: a weapon (physical attack) used by attack unit
//-------------------------------------------------------------

public class Melee : Weapon
{
    public Melee()
    {
        SetRange(5);
        SetSpeed(25);
        SetDamage(5);
        SetFiringRate(1.0f);
    }
}
