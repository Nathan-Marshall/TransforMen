using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeBehaviour : BaseBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.Ruin, GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Scavenge(GameObject unit, GameObject target)
    {
        GameObject controller = GameObject.Find("Game Control");
        PlayerResources resourceControl = controller.GetComponent<PlayerResources>();
        
        Ruin ruin = target.GetComponent<Ruin>();

        PopulationResource pop = ruin.GetPopulation();
        ScrapResource scrap = ruin.GetScrap();

        resourceControl.AddPopulation(pop);
        resourceControl.AddScrap(scrap);

        Destroy(target);
    }


    public override void PerformAction(GameObject unit, GameObject target)
    {
        unit.GetComponent<IndividualMovement>().moving = true;

        Collider moveCollider = target.GetComponent<Collider>();
        Vector3 destination = moveCollider.ClosestPoint(unit.transform.position);
        unit.GetComponent<IndividualMovement>().destination = destination;
        unit.GetComponent<IndividualMovement>().actionOnArrival = () => Scavenge(unit, target);
    }
}
