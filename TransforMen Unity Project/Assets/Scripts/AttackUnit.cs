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

    private IEnumerator attackRoutine;
    protected Animator animator;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.Enemy, GetType());
        animator = GetComponent<Animator>();
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
        if (target == null) {
            return;
        }

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }

        attackRoutine = SeekAndDestroy(target);

        StartCoroutine(attackRoutine);
    }

    IEnumerator SeekAndDestroy(GameObject attackTarget)
    {
        target = attackTarget.GetComponent<AttackTarget>();

        while (target.GetHealth() > 0)
        {
            Vector3 closestToTarget = GetComponent<Collider>().ClosestPoint(attackTarget.transform.position);
            Vector3 closestToThis = attackTarget.GetComponent<Collider>().ClosestPoint(transform.position);

            if (Vector3.Distance(closestToThis, closestToTarget) > weapon.GetRange())
            {
                animator.SetBool("Attacking", false);

                if (gameObject.GetComponent<IndividualMovement>().moving == false)
                {
                    GetComponent<IndividualMovement>().MoveTo(new Destination(attackTarget), null, true);
                }

                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                animator.SetBool("Attacking", true);
                target.TakeDamage(weapon.GetDamage());

                if (target.GetHealth() <= 0)
                {
                    animator.SetBool("Attacking", false);
                }

                Vector3 dir = (attackTarget.transform.position - transform.position).normalized;

                Quaternion orient = new Quaternion();
                orient.SetLookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up);
                orient *= Quaternion.AngleAxis(90, Vector3.up);
                transform.rotation = orient;

                yield return new WaitForSeconds(1.0f / weapon.GetFiringRate());
            }
        }
        animator.SetBool("Attacking", false);
        yield return null;
    }

    public System.Action GetAction(GameObject target) {
        return (() => Attack(target));
    }
    public System.Action GetStopAction()
    {
        return (() => { StopCoroutine(attackRoutine); animator.SetBool("Attacking", false); });
    }
}