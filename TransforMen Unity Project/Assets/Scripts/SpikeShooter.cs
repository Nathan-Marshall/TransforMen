using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Spike Shooter: a weapon used by attack unit to attack
//-------------------------------------------------------------

public class SpikeShooter : Weapon
{
    public SpikeShooter() {
        SetRange(20);
        SetSpeed(10);
        SetDamage(17);
        SetFiringRate(12.0f);
    }
}
