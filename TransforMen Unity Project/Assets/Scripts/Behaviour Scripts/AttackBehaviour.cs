using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : BaseBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.Enemy, GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(GameObject unit, GameObject target)
    {
        print("Attack");
    }

    public override void PerformAction(GameObject unit, GameObject target)
    {
        unit.GetComponent<IndividualMovement>().moving = true;

        Collider moveCollider = target.GetComponent<Collider>();
        Vector3 destination = moveCollider.ClosestPoint(unit.transform.position);
        unit.GetComponent<IndividualMovement>().destination = destination;
        unit.GetComponent<IndividualMovement>().actionOnArrival = () => Attack(unit, target);
    }
}
