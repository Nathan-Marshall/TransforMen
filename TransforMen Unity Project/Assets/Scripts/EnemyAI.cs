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
    public const float SENSE_FLEE_RANGE = 40.0f;
    public const float DEFEND_RADIUS = 50.0f;
    public const int FLEE_NUM_HUMANS_THRESHOLD = 5;

    public float attackRange;
    public int attackDamage;
    public float attackRate;

    private Animator animator;

    private bool canFlee = true;

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

        string name = gameObject.name;
        
        if (name.Contains("Crawler"))
        {
            attackDamage = 3;
            attackRate = 2.5f;
        }
        else if (name.Contains("Spike"))
        {
            attackDamage = 6;
            attackRate = 1;
        }
    }

    void Update()
    {
        if (nearbyHumanUnits.Count > 0)
        {
            if ((DetectWithTag("Ally", SENSE_FLEE_RANGE) - DetectWithTag("Enemy", SENSE_FLEE_RANGE)) > FLEE_NUM_HUMANS_THRESHOLD && canFlee)
            {
                if (state != EnemyState.FLEEING)
                {
                    ChangeEnemyState(EnemyState.FLEEING);
                }
            }
            else
            {
                GameObject nearestHuman = GetNearestObj(nearbyHumanUnits);

                if (nearestHuman != null)
                {
                    if (state != EnemyState.ATTACKING && state != EnemyState.FLEEING)
                    {
                        ChangeEnemyState(EnemyState.ATTACKING, nearestHuman);
                    }
                }
            }
        }

        else if (nearbyHumanBuildings.Count > 0)
        {
            GameObject nearestHumanBuilding = GetNearestObj(nearbyHumanBuildings);

            if (nearestHumanBuilding != null)
            {
                if (state != EnemyState.ATTACKING && state != EnemyState.FLEEING)
                {
                    ChangeEnemyState(EnemyState.ATTACKING, nearestHumanBuilding);
                }
            }
        }

        else if (state != EnemyState.DEFENDING && state != EnemyState.FLEEING && state != EnemyState.IDLE)
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
            if (obj != null)
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

            IndividualMovement movement = GetComponent<IndividualMovement>();

            if (movement != null)
            {
                if (movement.moving)
                {
                    movement.CancelMovement();
                }
            }

            animator.SetBool("Attacking", false);

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
                    currentRoutine = MoveToNearestSpawner();
                    StartCoroutine(currentRoutine);
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

    int DetectWithTag(string tag, float range = SENSE_RANGE)
    {
        return DetectWithTag(tag, transform.position, range);
    }

    int DetectWithTag (string tag, Vector3 sensePoint, float range = SENSE_RANGE)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(tag);

        int count = 0;
        for (int i = 0; i < units.Length; i++)
        {
            if (Vector3.Distance(units[i].transform.position, sensePoint) <= range)
            {
                if (units[i].GetComponent<IndividualMovement>() != null)
                {
                    count++;
                }
            }
        }
        return count;
    }

    IEnumerator MoveToNearestSpawner()
    {
        while (state == EnemyState.FLEEING)
        {
            Destination destination = null;
            IndividualMovement movement = gameObject.GetComponent<IndividualMovement>();
            while (destination == null || !movement.DestinationReachable(destination))
            {
                GameObject[] aliens = GameObject.FindGameObjectsWithTag("Enemy");
                List<GameObject> hives = new List<GameObject>();

                for (var i = 0; i < aliens.Length; i++)
                {
                    if (aliens[i].name.Contains("Hive"))
                    {
                        int nearbyHumans = DetectWithTag("Ally", aliens[i].transform.position, SENSE_RANGE);

                        if (nearbyHumans < FLEE_NUM_HUMANS_THRESHOLD)
                        {
                            hives.Add(aliens[i]);
                        }
                    }
                }

                if (hives.Count > 0)
                {
                    List<GameObject> reachableHives = new List<GameObject>();

                    foreach (GameObject hive in hives)
                    {
                        if (movement.DestinationReachable(new Destination(hive)))
                        {
                            reachableHives.Add(hive);
                        }
                    }

                    if (reachableHives.Count > 0)
                    {
                        destination = new Destination(GetNearestObj(reachableHives));
                    }
                    else
                    {
                        canFlee = false;
                        ChangeEnemyState(EnemyState.RANDOM_MOVING);
                        break;
                    }
                }
                else
                {
                    canFlee = false;
                    ChangeEnemyState(EnemyState.RANDOM_MOVING);
                    break;
                }
            }

            movement.MoveTo(destination, () => ChangeEnemyState(EnemyState.DEFENDING));

            while (movement.moving)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator MoveToRandomDestination()
    {
        while (state == EnemyState.RANDOM_MOVING)
        {
            GameObject hq = GameObject.Find("HQ");

            Destination destination = null;
            IndividualMovement movement = gameObject.GetComponent<IndividualMovement>();

            int count = 0;

            while (destination == null || !movement.DestinationReachable(destination) && count < 50)
            {
                int threat = (int)Time.fixedTime / 30;
                int seekHqChance = Random.Range(0, 50) + threat;

                if (seekHqChance > 40)
                {
                    destination = new Destination(hq);
                }
                else
                {
                    destination = new Destination(new Vector3(Random.Range(-500, 500), 0, Random.Range(-500, 500)));
                }

                if (Vector3.Distance(destination.Position, hq.transform.position) < 200.0f && Time.fixedTime < 120)
                {
                    destination = null;
                }

                count++;
            }

            if (count == 50 || destination == null)
            {
                ChangeEnemyState(EnemyState.IDLE);
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
