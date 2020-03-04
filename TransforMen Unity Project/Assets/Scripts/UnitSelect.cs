using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    public List<GameObject> selectedUnits;

    private Vector3 startPoint;
    private bool selecting = false;
    private Pivot currentPivot;

    enum Pivot
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkSelecting();
        
        if (selecting)
        {
            setSelectionPanel();
        }
    }

    void selectUnits(Vector3 p1, Vector3 p2)
    {
        Vector3 worldP1 = new Vector3(-99999, -99999, -99999);
        Vector3 worldP2 = new Vector3(-99999, -99999, -99999);

        GameObject selectionPlane = GameObject.Find("Selection Plane");
        Collider selectionPlaneCollider = selectionPlane.GetComponent<MeshCollider>();

        Ray p1Ray = Camera.main.ScreenPointToRay(p1);
        Ray p2Ray = Camera.main.ScreenPointToRay(p2);

        RaycastHit p1Hit, p2Hit;

        if (selectionPlaneCollider.Raycast(p1Ray, out p1Hit, 1000.0f))
        {
            worldP1 = p1Hit.point;
        }

        if (selectionPlaneCollider.Raycast(p2Ray, out p2Hit, 1000.0f))
        {
            worldP2 = p2Hit.point;
        }

        Vector3 leftPoint = new Vector3(-99999, -99999, -99999);
        Vector3 rightPoint = new Vector3(-99999, -99999, -99999);
        Vector3 topPoint = new Vector3(-99999, -99999, -99999);
        Vector3 bottomPoint = new Vector3(-99999, -99999, -99999);

        GameObject sphere1 = GameObject.Find("Sphere1");
        GameObject sphere2 = GameObject.Find("Sphere2");
        GameObject sphere3 = GameObject.Find("Sphere3");
        GameObject sphere4 = GameObject.Find("Sphere4");

        
        if (worldP1.x != 99999 && worldP2.x != 99999)
        {
            if (p1.x > p2.x)
            {
                rightPoint = worldP1;
                leftPoint = worldP2;
            }
            else
            {
                rightPoint = worldP2;
                leftPoint = worldP1;
            }

            if (p1.y > p2.y)
            {
                topPoint = worldP1;
                bottomPoint = worldP2;
            }
            else
            {
                topPoint = worldP2;
                bottomPoint = worldP1;
            }
        }

        GameObject[] units;
        List<GameObject> tempSelectedUnits = new List<GameObject>();
        
        units = GameObject.FindGameObjectsWithTag("Ally");

        bool in_x = false;
        bool in_z = false;
        
        foreach (GameObject unit in units)
        {
            if (unit.transform.position.x < rightPoint.x && unit.transform.position.x > leftPoint.x)
            {
                in_x = true;
            }

            if (unit.transform.position.z < topPoint.z && unit.transform.position.z > bottomPoint.z)
            {
                in_z = true;
            }

            if (in_x && in_z)
            {
                tempSelectedUnits.Add(unit);
            }
            
            in_x = false;
            in_z = false;
        }

        selectedUnits = tempSelectedUnits;
    }

    // Check to see if the player is currently selecting
    void checkSelecting()
    {
        GameObject selectorPane = GameObject.Find("Selector Pane");
        RectTransform selectorPaneRect = selectorPane.GetComponent<RectTransform>();

        // LMB Down
        if (Input.GetMouseButtonDown(0) && !selecting)
        {
            selecting = true;
            startPoint = Input.mousePosition;
        }
        // LMB Not down

        if (Input.GetMouseButtonUp(0) && selecting)
        {
            selecting = false;
            selectorPaneRect.sizeDelta = new Vector2(0, 0);

            selectUnits(startPoint, Input.mousePosition);

            startPoint = new Vector3(0, 0, 0);
        }
    }

    // Modify and move the unit selection panel with the mouse drag
    void setSelectionPanel()
    {
        GameObject selectorPane = GameObject.Find("Selector Pane");
        RectTransform selectorPaneRect = selectorPane.GetComponent<RectTransform>();

        Vector3 mousePos = Input.mousePosition;

        float xdiff = mousePos.x - startPoint.x;
        float ydiff = mousePos.y - startPoint.y;

        if (xdiff > 0 && ydiff < 0)
        {
            //Set to top left
            currentPivot = Pivot.TopLeft;
            selectorPaneRect.anchorMin = new Vector2(0, 1);
            selectorPaneRect.anchorMax = new Vector2(0, 1);
            selectorPaneRect.pivot = new Vector2(0, 1);
        }
        else if (xdiff < 0 && ydiff < 0)
        {
            //Set to top right
            currentPivot = Pivot.TopRight;
            selectorPaneRect.anchorMin = new Vector2(1, 1);
            selectorPaneRect.anchorMax = new Vector2(1, 1);
            selectorPaneRect.pivot = new Vector2(1, 1);
        }
        else if (xdiff < 0 && ydiff > 0)
        {
            //Set to bottom right
            currentPivot = Pivot.BottomRight;
            selectorPaneRect.anchorMin = new Vector2(1, 0);
            selectorPaneRect.anchorMax = new Vector2(1, 0);
            selectorPaneRect.pivot = new Vector2(1, 0);
        }
        else
        {
            //Set to bottom left
            currentPivot = Pivot.BottomLeft;
            selectorPaneRect.anchorMin = new Vector2(0, 0);
            selectorPaneRect.anchorMax = new Vector2(0, 0);
            selectorPaneRect.pivot = new Vector2(0, 0);
        }

        selectorPaneRect.sizeDelta = new Vector2(Mathf.Abs(mousePos.x - startPoint.x), Mathf.Abs(mousePos.y - startPoint.y));
        selectorPaneRect.position = startPoint;
    }
}
