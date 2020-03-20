﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformStation : StaticUnit
{
    public enum Upgrades
    {
        Spike,
        Crawler
    }

    public Transform SpikeDude;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.TransformStation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Upgrade a unit
    public void Upgrade(GameObject unit, Upgrades upgradeToType)
    {
        Vector3 spacingVec = (transform.position - unit.transform.position).normalized;
        Vector3 unitPosition = unit.transform.position;

        Destroy(unit);

        Vector3 newPos = new Vector3(unitPosition.x, SpikeDude.transform.position.y + 5, unitPosition.z) - (new Vector3(spacingVec.x, 0, spacingVec.z) * 5);

        Instantiate(SpikeDude, newPos, Quaternion.identity);
    }

}
