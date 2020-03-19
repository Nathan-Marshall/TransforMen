using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Attack Unit: a dynamic unit that is capable of attacking 
//-------------------------------------------------------------

public class AttackUnit : DynamicUnit, UnitAction
{
    //Collaborators: Dynamic Unit, Attack Target, Weapon 

    protected Weapon weapon; //the weapon this attacker has 
    protected bool canAttack; //whether or not the unit can attack 
    protected AttackTarget target = null; //the current target to attack 

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.Enemy, GetType());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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

    public void Attack(GameObject target) {
        this.target = target.GetComponent<AttackTarget>();
        target.GetComponent<AttackTarget>().TakeDamage(weapon.GetDamage());
    }

    public void PerformAction(GameObject target) {
        Collider moveCollider = target.GetComponent<Collider>();
        Vector3 destination = moveCollider.ClosestPoint(transform.position);

        GetComponent<IndividualMovement>().MoveTo(destination, () => Attack(target));

    }
}