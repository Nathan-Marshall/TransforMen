using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public enum TargetType
    {
        Ruin,
        Ally
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

        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            Vector3 worldPos = new Vector3(-99999, -99999, -99999);

            GameObject selectionPlane = GameObject.Find("Selection Plane");
            Collider selectionPlaneCollider = selectionPlane.GetComponent<MeshCollider>();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            string collidedWithName = "none";

            if (Physics.Raycast(ray, out hit))
            {
                collidedWithName = hit.collider.name;
            }

            if (collidedWithName != "none" && collidedWithName != "Terrain")
            {
                if (collidedWithName == "Selection Plane")
                {
                    worldPos = hit.point;

                    if (worldPos.x != -99999)
                    {
                        // Set the destination for the selected units
                        foreach (GameObject unit in selectedUnits)
                        {
                            unit.GetComponent<IndividualMovement>().moving = true;
                            unit.GetComponent<IndividualMovement>().destination = worldPos;
                        }
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
                                BaseBehaviour behaviourComponent = (BaseBehaviour)unit.GetComponent(behaviourMap[type]);

                                behaviourComponent.PerformAction(unit, hit.collider.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
}
