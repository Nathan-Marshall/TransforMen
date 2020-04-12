using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private PlacingBuilding placing;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (GameObject.Find("AlienMothership") == null || GameObject.Find("HQ") == null)
        {
            GameObject.Find("Canvas").transform.Find("Button Blocker").gameObject.SetActive(true);
            return;
        }
    }

    public void BeginPlacing(GameObject model) {
        if (placing != null) {
            placing.Cancel();
        }
        placing = Instantiate(model).GetComponent<PlacingBuilding>();
    }
}
