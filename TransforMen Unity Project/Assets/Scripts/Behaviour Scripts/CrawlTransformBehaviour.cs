using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlTransformBehaviour : MonoBehaviour, UnitAction
{
    // Start is called before the first frame update
    void Start()
    {
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.CrawlTransformStation, GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Transform(GameObject target)
    {
        //Check if we have enough resources to transform
        GameObject controller = GameObject.Find("Game Control");
        PlayerResources resourceControl = controller.GetComponent<PlayerResources>();

        if (resourceControl.GetCrawlbitResource() >= 5)
        {
            //Upgrade to crawler
            CrawlTransformStation.Upgrades upgradeToType = CrawlTransformStation.Upgrades.Crawler;
            resourceControl.SpendCrawlbits(5);

            target.GetComponent<CrawlTransformStation>().QueueUpgrade(gameObject, upgradeToType);
        }
        else
        {
            GameObject panel = GameObject.Find("Canvas").transform.Find("Lower Panel").transform.Find("Resource Panel").gameObject;
            panel.GetComponent<ResourcePanel>().showInsufficiency(0, 0, 0, 5);
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
