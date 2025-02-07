﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public bool gameOver; 

    public enum TargetType
    {
        Ruin,
        Ally,
        Enemy,
        SpikeTransformStation,
        CrawlTransformStation, 
        Hive,
        HQ
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("HQ") == null || GameObject.Find("AlienMothership") == null)
        {
            string endText;
            if (GameObject.Find("HQ") == null) {
                endText = "Defeat...";
            } else {
                endText = "VICTORY!";
            }
            GameObject.Find("Canvas").transform.Find("End Screen").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("End Screen").transform.Find("End Screen Text").GetComponent<TMPro.TextMeshProUGUI>().SetText(endText);

            GameObject[] allObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (GameObject child in allObjs) {
                if (child.tag.Equals("Enemy")) {
                    //Remove all resources so that when they die, you supply doesnt skyrocket
                    child.GetComponent<AttackTarget>().SetSpikeResource(0);
                    child.GetComponent<AttackTarget>().SetCrawlResource(0);
                    child.GetComponent<AttackTarget>().SetScrapResource(0);

                    //Kill all existing enemies 
                    child.GetComponent<AttackTarget>().TakeDamage(10000);
                }
            }
            return;
        }

        //Get the currently selected units
        List<GameObject> selectedUnits;
        UnitSelect selectScript = GameObject.Find("Game Control").GetComponent<UnitSelect>();
        selectedUnits = selectScript.selectedUnits;
        for (int i = 0; i < selectedUnits.Count; i++) {
            if (selectedUnits[i] == null) {
                selectedUnits.RemoveAt(i);
                i--;
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.name != "Terrain")
            {
                if (hit.collider.name == "Selection Plane")
                {
                    // Set the destination for the selected units
                    foreach (GameObject unit in selectedUnits)
                    {
                        //null check in case the unit was recently destroyed
                        if (unit != null)
                        {
                            unit.GetComponent<DynamicUnit>().stopAction?.Invoke();
                            unit.GetComponent<IndividualMovement>().MoveTo(new Destination(hit.point), null);
                        }
                    }
                }
                else
                {
                    GameObject target = hit.collider.gameObject;

                    foreach (GameObject unit in selectedUnits)
                    {
                        Dictionary<TargetType, System.Type> behaviourMap = unit.GetComponent<BehaviourMap>().behaviourMap;
                        List<TargetType> targetTypes = target.GetComponent<BehaviourMap>().targetTypes;

                        foreach (TargetType type in targetTypes)
                        {
                            if (behaviourMap.ContainsKey(type))
                            {
                                unit.GetComponent<DynamicUnit>().stopAction?.Invoke();

                                UnitAction behaviourComponent = (UnitAction)unit.GetComponent(behaviourMap[type]);
                                System.Action action = behaviourComponent.GetAction(target);
                                System.Action stopAction = behaviourComponent.GetStopAction();

                                unit.GetComponent<DynamicUnit>().stopAction = stopAction;

                                //Enemy movement is handled by seek and destroy behaviour
                                if (type == TargetType.Enemy)
                                {
                                    action();
                                }
                                else 
                                { 
                                    unit.GetComponent<IndividualMovement>().MoveTo(new Destination(target), action);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
