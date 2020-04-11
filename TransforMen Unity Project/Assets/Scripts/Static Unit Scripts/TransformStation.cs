using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformStation : StaticUnit
{
    private string desc = "Station where the most dedicated soldiers are evolved into something greater";

    private const float UPGRADE_TIME = 3.0f;

    private float currentUpgradeTime = UPGRADE_TIME;
    private List<System.Tuple<Vector3, Upgrades>> upgradeQueue = new List<System.Tuple<Vector3, Upgrades>>();

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
                "Upgrade Queue Length:", () => { return upgradeQueue.Count; },
                "Current Upgrade Time:", () => { return currentUpgradeTime; },
                "Cost:", string.Format("5 Spikes"),
                "Upgrade infantry to spike soldiers.\nSpike soldiers do more damage than normal infantry.\nSend units to this building to upgrade them",
                "", null);
        }

        if (currentUpgradeTime > 0 && upgradeQueue.Count > 0)
        {
            currentUpgradeTime -= Time.deltaTime;
            if (currentUpgradeTime < 0)
            {
                currentUpgradeTime = 0;
            }
        }

        if (upgradeQueue.Count > 0)
        {
            if (currentUpgradeTime == 0.0)
            {
                Upgrade(upgradeQueue[0]);
                upgradeQueue.RemoveAt(0);
                currentUpgradeTime = UPGRADE_TIME;
            }
        }
    }

    public void QueueUpgrade(GameObject unit, Upgrades upgradeToType)
    {
        Vector3 unitPosition = unit.transform.position;

        Destroy(unit);

        upgradeQueue.Add(new System.Tuple<Vector3, Upgrades>(unitPosition, upgradeToType));

        if (upgradeQueue.Count == 1 && currentUpgradeTime == 0)
        {
            currentUpgradeTime = UPGRADE_TIME;
        }
    }

    //Upgrade a unit
    public void Upgrade(System.Tuple<Vector3, Upgrades> upgradeInfo)
    {
        Vector3 unitPos = upgradeInfo.Item1;
        Upgrades upgradeToType = upgradeInfo.Item2;

        Vector3 spacingVec = (transform.position - unitPos).normalized;

        Vector3 newPos = new Vector3(unitPos.x, 10, unitPos.z) - (new Vector3(spacingVec.x, 0, spacingVec.z) * 5);

        Instantiate(SpikeDude, newPos, Quaternion.identity);
    }
}
