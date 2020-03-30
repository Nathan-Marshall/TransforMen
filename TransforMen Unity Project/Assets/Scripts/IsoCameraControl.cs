using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCameraControl : MonoBehaviour
{
    static readonly Vector2 MapSize = new Vector2(980, 980);
    const float MoveSpeed = 350;
    const float ScreenEdgeThreshold = 10;
    const float MouseWheelZoomFactor = 0.1f;
    const float MinCamHeight = 20;

    Camera cam;

    bool middleClick = false;
    Vector3 middleClickCamPos;
    Vector2 middleClickScreenPoint;

    float CamRY {
        get { return cam.transform.eulerAngles.y * Mathf.Deg2Rad; }
        set {
            cam.transform.rotation = Quaternion.Euler(cam.transform.eulerAngles.x, value * Mathf.Rad2Deg, 0);
        }
    }
    Quaternion CamRYQuat {
        get { return Quaternion.Euler(0, cam.transform.eulerAngles.y, 0); }
    }
    float CamRX {
        get { return cam.transform.eulerAngles.x * Mathf.Deg2Rad; }
        set {
            cam.transform.rotation = Quaternion.Euler(value * Mathf.Rad2Deg, cam.transform.eulerAngles.y, 0);
        }
    }

    Vector3 CamPos {
        get { return cam.transform.position; }
        set { cam.transform.position = value; }
    }
    float CamW {
        get { return cam.orthographicSize * 2 * cam.aspect; }
        set { cam.orthographicSize = value * 0.5f / cam.aspect; }
    }
    float CamH {
        get { return cam.orthographicSize * 2; }
        set { cam.orthographicSize = value * 0.5f; }
    }

    // Note: zoom level is 1 when the camera fits the entire width or height of the map, whichever dimension is smaller
    float Zoom {
        get { return Mathf.Min(MapSize.x / CamW, MapSize.y / ProjectedHeight); } // change depending on orientation
        set { CamH = Mathf.Min(MapSize.x / cam.aspect, MapSize.y * Mathf.Sin(CamRX)) / value; } // change depending on orientation
    }

    Vector2 ProjectedCenter {
        get {
            float cDist = CamPos.y / Mathf.Tan(CamRX);
            return new Vector3(CamPos.x, CamPos.z + cDist); // change depending on orientation
        }
        set {
            float cDist = CamPos.y / Mathf.Tan(CamRX);
            CamPos = new Vector3(value.x, CamPos.y, value.y - cDist); // change depending on orientation
        }
    }
    float ProjectedHeight {
        get { return CamH / Mathf.Sin(CamRX); }
        set { CamH = value * Mathf.Sin(CamRX); }
    }
    float ProjectedLeft {
        get { return ProjectedCenter.x - CamW * 0.5f; } // change depending on orientation
        set { ProjectedCenter = new Vector2(value + CamW * 0.5f, ProjectedCenter.y); } // change depending on orientation
    }
    float ProjectedRight {
        get {  return ProjectedCenter.x + CamW * 0.5f; } // change depending on orientation
        set { ProjectedCenter = new Vector2(value - CamW * 0.5f, ProjectedCenter.y); } // change depending on orientation
    }
    float ProjectedBottom {
        get { return ProjectedCenter.y - ProjectedHeight * 0.5f; } // change depending on orientation
        set { ProjectedCenter = new Vector2(ProjectedCenter.x, value + ProjectedHeight * 0.5f); } // change depending on orientation
    }
    float ProjectedTop {
        get { return ProjectedCenter.y + ProjectedHeight * 0.5f; } // change depending on orientation
        set { ProjectedCenter = new Vector2(ProjectedCenter.x, value - ProjectedHeight * 0.5f); } // change depending on orientation
    }

    Vector2 ProjectedMouse {
        get {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 scaledDir = ray.direction * (ray.origin.y / -ray.direction.y);
            return new Vector2(ray.origin.x + scaledDir.x, ray.origin.z + scaledDir.z); // change depending on orientation???
        }
    }

    Vector3 Left { get { return new Vector3(-Mathf.Cos(CamRY), 0, -Mathf.Sin(CamRY)); } }
    Vector3 Right { get { return new Vector3(Mathf.Cos(CamRY), 0, Mathf.Sin(CamRY)); } }
    Vector3 Down { get { return new Vector3(-Mathf.Sin(CamRY), 0, -Mathf.Cos(CamRY)); } }
    Vector3 Up { get { return new Vector3(Mathf.Sin(CamRY), 0, Mathf.Cos(CamRY)); } }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        Zoom = 5;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle translations
        if (Input.GetMouseButtonDown(2)) {
            middleClick = true;
            middleClickCamPos = CamPos;
            middleClickScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(2)) {
            middleClick = false;
        }
        if (middleClick) {
            // Dragging to pan
            Vector2 screenDiff = middleClickScreenPoint - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 projectedDiff = screenDiff / Screen.height * CamH;
            projectedDiff.y /= Mathf.Sin(CamRX);
            CamPos = middleClickCamPos + new Vector3(projectedDiff.x, 0, projectedDiff.y); // change depending on orientation
        } else {
            // Not dragging

            if (Input.mousePosition.x < ScreenEdgeThreshold || Input.GetKey(KeyCode.LeftArrow)) {
                cam.transform.position += MoveSpeed / Zoom * Time.deltaTime * Left;
            }
            if (Input.mousePosition.x > Screen.width - ScreenEdgeThreshold || Input.GetKey(KeyCode.RightArrow)) {
                cam.transform.position += MoveSpeed / Zoom * Time.deltaTime * Right;
            }

            if (Input.mousePosition.y < ScreenEdgeThreshold || Input.GetKey(KeyCode.DownArrow)) {
                cam.transform.position += MoveSpeed / Zoom * Time.deltaTime * Down;
            }
            if (Input.mousePosition.y > Screen.height - ScreenEdgeThreshold || Input.GetKey(KeyCode.UpArrow)) {
                cam.transform.position += MoveSpeed / Zoom * Time.deltaTime * Up;
            }

            // Handle zooming
            float oldZoom = Zoom;
            Vector2 fromCenter = new Vector2();
            if (Input.mouseScrollDelta.y != 0) {
                fromCenter = ProjectedMouse - ProjectedCenter;
                Zoom *= (1 + Input.mouseScrollDelta.y * MouseWheelZoomFactor);
            }

            // Min zoom
            if (CamH < MinCamHeight) {
                CamH = MinCamHeight;
            }

            // Apply zoom
            ProjectedCenter += fromCenter * (1 - oldZoom / Zoom);

            // Max zoom (adjust center to check new bounds, then adjust back if needed)
            if (CamW > MapSize.x || ProjectedHeight > MapSize.y) { // change depending on orientation
                ProjectedCenter -= fromCenter * (1 - oldZoom / Zoom);
                Zoom = 1;
                ProjectedCenter += fromCenter * (1 - oldZoom / Zoom);
            }
        }

        // Bound translations
        if (ProjectedLeft < -MapSize.x * 0.5f) { // change depending on orientation
            ProjectedLeft = -MapSize.x * 0.5f; // change depending on orientation
        }
        if (ProjectedRight > MapSize.x * 0.5f) { // change depending on orientation
            ProjectedRight = MapSize.x * 0.5f; // change depending on orientation
        }
        if (ProjectedBottom < -MapSize.y * 0.5f) { // change depending on orientation
            ProjectedBottom = -MapSize.y * 0.5f; // change depending on orientation
        }
        if (ProjectedTop > MapSize.y * 0.5f) { // change depending on orientation
            ProjectedTop = MapSize.y * 0.5f; // change depending on orientation
        }
    }
}
