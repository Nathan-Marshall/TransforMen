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
        SetDamage(28);
        SetFiringRate(6.0f);
    }
}
