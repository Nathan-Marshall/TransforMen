using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Machine Gun: a weapon used by attack unit to attack
//-------------------------------------------------------------

public class MachineGun : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        SetRange(15);
        SetSpeed(15);
        SetDamage(15);
        SetFiringRate(15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
