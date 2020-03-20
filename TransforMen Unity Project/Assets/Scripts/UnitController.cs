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
        TransformStation
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
                bool solo = true;

                if (selectedUnits.Count > 1)
                {
                    solo = false;
                }

                if (hit.collider.name == "Selection Plane")
                {
                    // Set the destination for the selected units
                    foreach (GameObject unit in selectedUnits)
                    {
                        unit.GetComponent<IndividualMovement>().MoveTo(hit.point, null, solo);
                    }
                }
                else
                {
                    foreach (GameObject unit in selectedUnits)
                    {
                        Dictionary<TargetType, System.Type> behaviourMap = unit.GetComponent<BehaviourMap>().behaviourMap;
                        List<TargetType> targetTypes = hit.collider.gameObject.GetComponent<BehaviourMap>().targetTypes;

                        foreach (TargetType type in targetTypes)
                        {
                            if (behaviourMap.ContainsKey(type))
                            {
                                UnitAction behaviourComponent = (UnitAction)unit.GetComponent(behaviourMap[type]);
                                System.Action action = behaviourComponent.GetAction(hit.collider.gameObject);

                                Collider moveCollider = hit.collider.gameObject.GetComponent<Collider>();
                                Vector3 destination = moveCollider.ClosestPoint(unit.transform.position);

                                unit.GetComponent<IndividualMovement>().MoveTo(destination, action, solo);
                            }
                        }
                    }
                }
            }
        }
    }
}
