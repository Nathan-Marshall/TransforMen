using System.Collections;
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
        Vector3 position = unit.transform.position;

        Destroy(unit);

        Instantiate(SpikeDude, position, Quaternion.identity);
    }

}
