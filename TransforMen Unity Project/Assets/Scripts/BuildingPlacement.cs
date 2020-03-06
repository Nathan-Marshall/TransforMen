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
        
    }

    public void BeginPlacing(GameObject model) {
        if (placing != null) {
            placing.Cancel();
        }
        placing = Instantiate(model).GetComponent<PlacingBuilding>();
    }
}
