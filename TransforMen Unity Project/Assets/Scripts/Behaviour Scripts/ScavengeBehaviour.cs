﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeBehaviour : BaseBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.Ruin, GetType());

        print(mapping.behaviourMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Scavenge(GameObject unit, GameObject target)
    {
        GameObject controller = GameObject.Find("Game Control");
        PlayerResources resourceControl = controller.GetComponent<PlayerResources>();

        print("Population: " + resourceControl.GetPopulationResource());
        print("Scrap: " + resourceControl.GetScrapResource());

        Ruin ruin = target.GetComponent<Ruin>();

        int pop = ruin.GetPopulation();
        int scrap = ruin.GetScrap();

        resourceControl.AddPopulation(pop);
        resourceControl.AddScrap(scrap);

        print("Population: " + resourceControl.GetPopulationResource());
        print("Scrap: " + resourceControl.GetScrapResource());

        Destroy(target);
    }


    public override void PerformAction(GameObject unit, GameObject target)
    {
        unit.GetComponent<IndividualMovement>().moving = true;

        Collider moveCollider = target.GetComponent<Collider>();
        Vector3 destination = moveCollider.ClosestPointOnBounds(unit.transform.position);
        unit.GetComponent<IndividualMovement>().destination = destination;
        unit.GetComponent<IndividualMovement>().actionOnArrival = () => Scavenge(unit, target);
    }
}
