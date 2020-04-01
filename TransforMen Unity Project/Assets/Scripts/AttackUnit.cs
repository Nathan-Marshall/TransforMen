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

    private IEnumerator currentRoutine;

    protected Animator animator;

    private enum AttackUnitState { ATTACKING, DEFENDING, IDLE };
    private AttackUnitState state;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.Enemy, GetType());
        animator = GetComponent<Animator>();
        state = AttackUnitState.DEFENDING;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (state == AttackUnitState.DEFENDING)
        {
            Defend();
        }
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

    private void ChangeState(AttackUnitState toState)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        animator.SetBool("Attacking", false);
        target = null;

        state = toState;
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

        ChangeState(AttackUnitState.ATTACKING);
        currentRoutine = SeekAndDestroy(target);
        StartCoroutine(currentRoutine);
    }

    public void Defend()
    {
        IndividualMovement movement = gameObject.GetComponent<IndividualMovement>();

        if (movement.moving && currentRoutine != null)
        {
            ChangeState(AttackUnitState.DEFENDING);
        }

        // If we are not currently running a defend routine, we should start one now
        else if (currentRoutine == null && movement != null && movement.moving == false)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(this.transform.position, weapon.GetRange());

            List<GameObject> nearbyEnemies = new List<GameObject>();
            foreach (Collider col in nearbyColliders)
            {
                GameObject colObj = col.gameObject;
                if (colObj.tag == "Enemy")
                {
                    nearbyEnemies.Add(colObj);
                }
            }

            if (nearbyEnemies.Count > 0)
            {
                GameObject nearestEnemy = null;
                float nearestDist = 999999;

                foreach (GameObject enemy in nearbyEnemies)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) < nearestDist)
                    {
                        if (enemy.GetComponent<AttackTarget>().GetHealth() > 0)
                        {
                            nearestEnemy = enemy;
                            nearestDist = Vector3.Distance(enemy.transform.position, transform.position);
                        }
                    }
                }

                if (nearestEnemy != null)
                {
                    currentRoutine = DefendingAttack(nearestEnemy);
                    StartCoroutine(currentRoutine);
                }
            }
        }
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

                transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up);

                yield return new WaitForSeconds(1.0f / weapon.GetFiringRate());
            }
        }
        animator.SetBool("Attacking", false);
        state = AttackUnitState.DEFENDING;
        yield return null;
    }


    IEnumerator DefendingAttack(GameObject attackTarget)
    {
        target = attackTarget.GetComponent<AttackTarget>();

        while (target.GetHealth() > 0)
        {
            Vector3 closestToTarget = GetComponent<Collider>().ClosestPoint(attackTarget.transform.position);
            Vector3 closestToThis = attackTarget.GetComponent<Collider>().ClosestPoint(transform.position);

            if (Vector3.Distance(closestToThis, closestToTarget) > weapon.GetRange())
            {
                //We stop the current defending corouting since the target moved out of range
                ChangeState(AttackUnitState.DEFENDING);
                yield return null;
            }
            else
            {
                animator.SetBool("Attacking", true);
                target.TakeDamage(weapon.GetDamage());

                Vector3 dir = (attackTarget.transform.position - transform.position).normalized;

                transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up);

                if (target.GetHealth() <= 0)
                {
                    ChangeState(AttackUnitState.DEFENDING);
                }

                yield return new WaitForSeconds(1.0f / weapon.GetFiringRate());
            }
        }
        ChangeState(AttackUnitState.DEFENDING);
        yield return null;
    }


    public System.Action GetAction(GameObject target) {
        return (() => Attack(target));
    }
    public System.Action GetStopAction()
    {
        return (() => { ChangeState(AttackUnitState.DEFENDING); });
    }
}