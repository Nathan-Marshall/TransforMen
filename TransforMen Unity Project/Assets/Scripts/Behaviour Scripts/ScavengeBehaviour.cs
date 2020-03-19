using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeBehaviour : MonoBehaviour, UnitAction
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
    
    public void Scavenge(GameObject target)
    {
        if (target == null) {
            return;
        }

        GameObject controller = GameObject.Find("Game Control");
        PlayerResources resourceControl = controller.GetComponent<PlayerResources>();
        Ruin ruin = target.GetComponent<Ruin>();

        int pop = ruin.GetPopulation();
        int scrap = ruin.GetScrap();

        resourceControl.AddPopulation(pop);
        resourceControl.AddScrap(scrap);

        Destroy(target);
    }


    public void PerformAction(GameObject target)
    {
        GetComponent<IndividualMovement>().moving = true;

        Collider moveCollider = target.GetComponent<Collider>();
        Vector3 destination = moveCollider.ClosestPoint(transform.position);
        GetComponent<IndividualMovement>().destination = destination;
        GetComponent<IndividualMovement>().actionOnArrival = () => Scavenge(target);
    }
}
