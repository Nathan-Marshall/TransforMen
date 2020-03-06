using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBehaviour : BaseBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.TransformStation, GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO: Add more logic in here for transformations
    public void Transform(GameObject unit, GameObject target)
    {
        //Check if we have enough resources to transform
        GameObject controller = GameObject.Find("Game Control");
        PlayerResources resourceControl = controller.GetComponent<PlayerResources>();

        //REQUIREMENTS FOR AN UPGRADE ARE DIFFERENT THAN THIS, THIS IS FOR TESTING
        if (resourceControl.GetScrapResource() >= 5)
        {

            //Here we need to check which type we are and what we want to upgrade to
            //For now, default to an infantry upgrading to a spike dude
            TransformStation.Upgrades upgradeToType = TransformStation.Upgrades.Spike;
            resourceControl.SpendScrap(5);

            target.GetComponent<TransformStation>().Upgrade(unit, upgradeToType);
        }
    }

    public override void PerformAction(GameObject unit, GameObject target)
    {
        unit.GetComponent<IndividualMovement>().moving = true;

        Collider moveCollider = target.GetComponent<Collider>();
        Vector3 destination = moveCollider.ClosestPoint(unit.transform.position);

        unit.GetComponent<IndividualMovement>().destination = destination;
        unit.GetComponent<IndividualMovement>().actionOnArrival = () => Transform(unit, target);
    }
}
