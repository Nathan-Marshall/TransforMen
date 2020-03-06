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
        SetRange(15);
        SetSpeed(15);
        SetDamage(10);
        SetFiringRate(15.0f);
    }
}
