﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTransformBehaviour : MonoBehaviour, UnitAction
{
    // Start is called before the first frame update
    void Start()
    {
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.SpikeTransformStation, GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO: Add more logic in here for transformations
    public void Transform(GameObject target)
    {
        //Check if we have enough resources to transform
        GameObject controller = GameObject.Find("Game Control");
        PlayerResources resourceControl = controller.GetComponent<PlayerResources>();

        if (resourceControl.GetSpikeResource() >= 5)
        {
            //Upgrade to spike 
            SpikeTransformStation.Upgrades upgradeToType = SpikeTransformStation.Upgrades.Spike; 
            resourceControl.SpendSpikes(5);

            target.GetComponent<SpikeTransformStation>().QueueUpgrade(gameObject, upgradeToType); 
        }
        else
        {
            GameObject panel = GameObject.Find("Canvas").transform.Find("Lower Panel").transform.Find("Resource Panel").gameObject;
            panel.GetComponent<ResourcePanel>().showInsufficiency(0, 0, 5, 0);
        }
    }

    public System.Action GetAction(GameObject target)
    {
        return (() => Transform(target));
    }

    public System.Action GetStopAction()
    {
        return null;
    }
}
