using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public enum TargetType
    {
        Ruin,
        Ally,
        Enemy,
        TransformStation,
        Hive
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

                GameObject leaderObj = selectedUnits[0];
                float minDist = 99999999;

                foreach (GameObject unit in selectedUnits)
                {
                    float dist = Vector3.Distance(unit.transform.position, hit.point);
                    if (dist < minDist)
                    {
                        leaderObj = unit;
                        minDist = dist;
                    }
                }

                if (hit.collider.name == "Selection Plane")
                {
                    // Set the destination for the selected units
                    foreach (GameObject unit in selectedUnits)
                    {
                        unit.GetComponent<DynamicUnit>().stopAction?.Invoke();

                        if (unit == leaderObj)
                        {
                            unit.GetComponent<IndividualMovement>().MoveTo(new Destination(hit.point), null, null, selectedUnits.Count == 1, true);
                        }
                        else
                        {
                            unit.GetComponent<IndividualMovement>().MoveTo(new Destination(leaderObj), new Destination(hit.point), null, selectedUnits.Count == 1);
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
                                    if (unit == leaderObj)
                                    {
                                        unit.GetComponent<IndividualMovement>().MoveTo(new Destination(target), null, action, selectedUnits.Count == 1, true);
                                    }
                                    else
                                    {
                                        unit.GetComponent<IndividualMovement>().MoveTo(new Destination(leaderObj), new Destination(target), action, selectedUnits.Count == 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
