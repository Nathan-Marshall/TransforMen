using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Spike Alien: dynamic unit that can attack human targets. 
//    is a simple melee unit that deals moderate damage 
//-------------------------------------------------------------

public class SpikeAlien : AttackUnit
{
    private string desc = "Spiky alien boys";
    //Collaborators: Dynamic Unit, Attack Target, Attack Unit 

    //AI-Controlled

    // Start is called before the first frame update
    void Start()
    {
        SetToAlienTeam();
        SetSelectable(false);
        SetDescription(desc);
        SetTransportable(true);
        SetMoveSpeed(5);
        SetCanScavenge(false);
        SetControllable(false);
        SetCanAttack(true);
        SetWeapon(new SpikeShooter());
        SetAttackTarget(null);

        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.Enemy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
