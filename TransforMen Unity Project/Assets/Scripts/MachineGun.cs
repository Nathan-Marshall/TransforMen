using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Machine Gun: a weapon used by attack unit to attack
//-------------------------------------------------------------

public class MachineGun : Weapon
{
    public MachineGun()
    {
        SetRange(40);
        SetSpeed(15);
        SetDamage(1);
        SetFiringRate(15.0f);
    }
}
