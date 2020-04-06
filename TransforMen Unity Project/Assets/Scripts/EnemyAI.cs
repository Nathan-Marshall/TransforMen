using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private List<GameObject> nearbyHumanUnits;
    private List<GameObject> nearbyHumanBuildings;
    private List<GameObject> nearbyAlienUnits;
    private List<GameObject> nearbyAlienBuildings;

    public const float SENSE_RANGE = 100.0f;
    public const float DEFEND_RADIUS = 50.0f;
    
    public float attackRange;
    public int attackDamage;
    public float attackRate;

    private Animator animator;

    public enum EnemyState { RANDOM_MOVING, ATTACKING, FLEEING, DEFENDING, IDLE };
    private EnemyState state;

    private IEnumerator currentRoutine = null;

    void Start()
    {
        animator = GetComponent<Animator>();

        nearbyHumanUnits = new List<GameObject>();
        nearbyHumanBuildings = new List<GameObject>();
        nearbyAlienUnits = new List<GameObject>();
        nearbyAlienBuildings = new List<GameObject>();

        //Constantly update the list of things near this enemy
        InvokeRepeating("DetectArea", 0.0f, 2.0f);
    }

    void Update()
    {
        if (nearbyHumanUnits.Count > 0)
        {
            GameObject nearestHuman = GetNearestObj(nearbyHumanUnits);

            if (nearestHuman != null)
            {
                if (state != EnemyState.ATTACKING)
                {
                    ChangeEnemyState(EnemyState.ATTACKING, nearestHuman);
                }
            }
        }

        else if (nearbyHumanBuildings.Count > 0)
        {
            GameObject nearestHumanBuilding = GetNearestObj(nearbyHumanBuildings);

            if (nearestHumanBuilding != null)
            {
                if (state != EnemyState.ATTACKING)
                {
                    ChangeEnemyState(EnemyState.ATTACKING, nearestHumanBuilding);
                }
            }
        }

        else if (state != EnemyState.DEFENDING)
        {
            ChangeEnemyState(EnemyState.RANDOM_MOVING);
        }
    }

    GameObject GetNearestObj(List<GameObject> objects)
    {
        GameObject nearestObj = null;
        float nearestDist = 999999;
        foreach (GameObject obj in objects)
        {
            if (Vector3.Distance(obj.transform.position, transform.position) < nearestDist)
            {
                if (obj.GetComponent<AttackTarget>() != null)
                {
                    if (obj.GetComponent<AttackTarget>().GetHealth() > 0)
                    {
                        nearestObj = obj;
                        nearestDist = Vector3.Distance(obj.transform.position, transform.position);
                    }
                }
            }
        }

        return nearestObj;
    }

    public void ChangeEnemyState(EnemyState newState, GameObject target = null)
    {
        if (newState != state || currentRoutine == null)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }

            if (state == EnemyState.RANDOM_MOVING)
            {
                IndividualMovement movement = GetComponent<IndividualMovement>();

                if (movement.moving)
                {
                    movement.CancelMovement();
                }
            }
            else if (state == EnemyState.ATTACKING)
            {
                animator.SetBool("Attacking", false);
            }
            else if (state == EnemyState.FLEEING)
            {
                return;
            }

            state = newState;

            switch (newState)
            {
                case EnemyState.RANDOM_MOVING:
                    currentRoutine = MoveToRandomDestination();
                    StartCoroutine(currentRoutine);
                    break;

                case EnemyState.ATTACKING:
                    currentRoutine = EnemyAttack(target);
                    StartCoroutine(currentRoutine);
                    break;

                case EnemyState.FLEEING:
                    break;

                default:
                    break;
            }
        }
    }

    //Fill the 4 lists of alien and human units/buildings with what is around this unit
    void DetectArea()
    {
        nearbyHumanUnits.Clear();
        nearbyHumanBuildings.Clear();
        nearbyAlienUnits.Clear();
        nearbyAlienBuildings.Clear();

        GameObject[] humans = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] aliens = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < aliens.Length; i++)
        {
            if (Vector3.Distance(aliens[i].transform.position, transform.position) <= SENSE_RANGE && aliens[i] != this.gameObject)
            {
                if (aliens[i].GetComponent<Hive>() != null)
                {
                    nearbyAlienBuildings.Add(aliens[i]);
                }
                else
                {
                    nearbyAlienUnits.Add(aliens[i]);
                }
            }
        }

        for (int i = 0; i < humans.Length; i++)
        {
            if (Vector3.Distance(humans[i].transform.position, transform.position) <= SENSE_RANGE)
            {
                if (humans[i].GetComponent<IndividualMovement>() != null)
                {
                    nearbyHumanUnits.Add(humans[i]);
                }
                else
                {
                    nearbyHumanBuildings.Add(humans[i]);
                }
            }
        }
    }

    IEnumerator MoveToRandomDestination()
    {
        while (state == EnemyState.RANDOM_MOVING)
        {
            Destination destination = null;
            IndividualMovement movement = gameObject.GetComponent<IndividualMovement>();
            while (destination == null || !movement.DestinationReachable(destination))
            {
                destination = new Destination(new Vector3(Random.Range(-500, 500), 0, Random.Range(-500, 500)));
            }

            movement.MoveTo(destination, null);

            while (movement.moving)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator EnemyAttack(GameObject attackTarget)
    {
        AttackTarget target = attackTarget.GetComponent<AttackTarget>();

        while (target.GetHealth() > 0)
        {
            Vector3 closestToTarget = GetComponent<Collider>().ClosestPoint(attackTarget.transform.position);
            Vector3 closestToThis = attackTarget.GetComponent<Collider>().ClosestPoint(transform.position);

            if (Vector3.Distance(closestToThis, closestToTarget) > attackRange)
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
                target.TakeDamage(attackDamage);

                if (target.GetHealth() <= 0)
                {
                    ChangeEnemyState(EnemyState.RANDOM_MOVING);
                }

                Vector3 dir = (attackTarget.transform.position - transform.position).normalized;

                transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up);

                yield return new WaitForSeconds(1.0f / attackRate);
            }
        }
        ChangeEnemyState(EnemyState.RANDOM_MOVING);
        yield return null;
    }
}
