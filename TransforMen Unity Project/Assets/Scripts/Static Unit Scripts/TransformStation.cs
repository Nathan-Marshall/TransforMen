using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformStation : StaticUnit
{
    private string desc = "Station where the most dedicated soldiers are evolved into something greater";

    private int queueLength = 0;
    private float currentTrainTime = 0.0f;

    public enum Upgrades
    {
        Spike,
        Crawler
    }

    public GameObject SpikeDude;

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(true);
        SetDescription(desc);
        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.TransformStation);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        if (boxCollider.Raycast(ray, out RaycastHit hit, 10000.0f) && Input.GetMouseButtonDown(0))
        {
            GameObject.Find("Game Control").GetComponent<PanelControl>().SetInfo(
                "Upgrade Queue Length:", () => { return queueLength; },
                "Current Upgrade Time:", () => { return currentTrainTime; },
                "Cost:", string.Format("5 Spikes"),
                "Upgrade infantry to spike soldiers.\nSpike soldiers do more damage than normal infantry.\nSend units to this building to upgrade them",
                "", null);
        }
    }

    //Upgrade a unit
    public void Upgrade(GameObject unit, Upgrades upgradeToType)
    {
        Vector3 spacingVec = (transform.position - unit.transform.position).normalized;
        Vector3 unitPosition = unit.transform.position;

        Destroy(unit);

        Vector3 newPos = new Vector3(unitPosition.x, 10, unitPosition.z) - (new Vector3(spacingVec.x, 0, spacingVec.z) * 5);

        Instantiate(SpikeDude, newPos, Quaternion.identity);
    }

}
