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
        SetRange(50);
        SetSpeed(15);
        SetDamage(3);
        SetFiringRate(7.5f);
    }
}
