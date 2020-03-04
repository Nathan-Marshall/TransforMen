using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
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

            if (selectionPlaneCollider.Raycast(ray, out hit, 1000.0f))
            {
                worldPos = hit.point;
            }

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
    }
    
}
