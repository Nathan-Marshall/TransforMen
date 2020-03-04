using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Attack Unit: a dynamic unit that is capable of attacking 
//-------------------------------------------------------------

public class AttackUnit : DynamicUnit
{
    //Collaborators: Dynamic Unit, Attack Target, Weapon 

    protected Weapon weapon; //the weapon this attacker has 
    protected bool canAttack; //whether or not the unit can attack 
    protected AttackTarget target = null; //the current target to attack 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get & Set CanAttack
    public bool GetCanAttack() { return canAttack; }
    protected void SetCanAttack(bool ifCanAttack)
    {
        canAttack = ifCanAttack;
    }

    //Get & Set Weapon
    public Weapon GetWeapon() { return weapon; }
    protected void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
    }

    public AttackTarget GetAttackTarget() { return target; }
    protected void SetAttackTarget(AttackTarget newTarget)
    {
        target = newTarget;
    }
}