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
        GameObject selectionPlane = GameObject.Find("Selection Plane");
        Collider selectionPlaneCollider = selectionPlane.GetComponent<MeshCollider>();

        Ray p1Ray = Camera.main.ScreenPointToRay(p1);
        Ray p2Ray = Camera.main.ScreenPointToRay(p2);

        selectionPlaneCollider.Raycast(p1Ray, out RaycastHit p1Hit, 1000.0f);
        selectionPlaneCollider.Raycast(p2Ray, out RaycastHit p2Hit, 1000.0f);

        GameObject[] units;
        List<GameObject> tempSelectedUnits = new List<GameObject>();
        
        units = GameObject.FindGameObjectsWithTag("Ally");

        // If both hits exist
        if (p1Hit.transform && p2Hit.transform) {
            float left = Mathf.Min(p1Hit.point.x, p2Hit.point.x);
            float right = Mathf.Max(p1Hit.point.x, p2Hit.point.x);
            float bottom = Mathf.Min(p1Hit.point.z, p2Hit.point.z);
            float top = Mathf.Max(p1Hit.point.z, p2Hit.point.z);

            foreach (GameObject unit in units) {
                Vector3 pos = unit.transform.position;
                if (pos.x > left && pos.x < right && pos.z > bottom && pos.z < top) {
                    tempSelectedUnits.Add(unit);
                }
            }
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
