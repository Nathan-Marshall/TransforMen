using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeBehaviour : BaseBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.RUIN, GetType());

        print(mapping.behaviourMap);

        mapping.targetTypes.Add(UnitController.TargetType.RUIN);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PerformAction(GameObject unit, GameObject target)
    {
        print("Scavenge");
    }
}
