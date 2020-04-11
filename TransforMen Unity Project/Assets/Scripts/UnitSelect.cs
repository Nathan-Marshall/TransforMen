using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    public List<GameObject> selectedUnits;

    private Collider selectionPlaneCollider;
    private Camera cam;
    private RectTransform selectorPaneRect;

    private Vector3 startPoint;
    private bool selecting = false;
    private Pivot currentPivot;

    private Vector3 ClampedMousePosition {
        get {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x = Mathf.Clamp(mousePos.x, cam.pixelRect.xMin, cam.pixelRect.xMax);
            mousePos.y = Mathf.Clamp(mousePos.y, cam.pixelRect.yMin, cam.pixelRect.yMax);
            return mousePos;
        }
    }

    enum Pivot
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    // Start is called before the first frame update
    void Start() {
        selectionPlaneCollider = GameObject.Find("Selection Plane").GetComponent<MeshCollider>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        selectorPaneRect = GameObject.Find("Selector Pane").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("AlienMothership") == null || GameObject.Find("HQ") == null)
        {
            return; 
        }

            checkSelecting();
        
        if (selecting)
        {
            selectUnits(startPoint, Input.mousePosition);
            setSelectionPanel();
        }
    }

    void selectUnits(Vector3 p1, Vector3 p2)
    {
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
        // LMB Down
        if (Input.GetMouseButtonDown(0) && !selecting && cam.pixelRect.Contains(Input.mousePosition)) {
            startPoint = ClampedMousePosition;
            selecting = true;
        }

        // LMB Up
        if (Input.GetMouseButtonUp(0) && selecting) {
            selectUnits(startPoint, ClampedMousePosition);
            selectorPaneRect.sizeDelta = new Vector2(0, 0);
            selecting = false;
        }
    }

    // Modify and move the unit selection panel with the mouse drag
    void setSelectionPanel()
    {
        Vector3 mousePos = ClampedMousePosition;

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
