using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingBuilding : MonoBehaviour
{
    public Material validMaterial;
    public Material invalidMaterial;
    public GameObject buildingPrefab;
    public int populationCost;
    public int scrapCost;
    public int spikesCost;
    public int crawlbitsCost;

    public bool Valid {
        get { return Affordable && activeCollisions.Count == 0; }
    }

    public bool Affordable {
        get {
            return resources.GetPopulationResource() >= populationCost
                    && resources.GetScrapResource() >= scrapCost
                    && resources.GetSpikeResource() >= spikesCost
                    && resources.GetCrawlbitResource() >= crawlbitsCost;
        }
    }

    private Collider terrainCollider;
    private Camera cam;
    private PlayerResources resources;

    private List<Collider> activeCollisions = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        terrainCollider = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        resources = GameObject.Find("Game Control").GetComponent<PlayerResources>();
        UpdateMaterials();
    }

    // Update is called once per frame
    void Update() {
        // Position the ghost of the model
        if (cam.pixelRect.Contains(Input.mousePosition)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            terrainCollider.Raycast(ray, out RaycastHit hit, 1000.0f);
            transform.position = hit.point + new Vector3(0, GetComponent<Collider>().bounds.size.y * 0.5f, 0);
        }

        if (Input.GetMouseButtonDown(0)) {
            if (cam.pixelRect.Contains(Input.mousePosition)) {
                Place();
            }
            else {
                Cancel();
            }
        } else if (Input.GetMouseButtonDown(1)) {
            Cancel();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (IsBlocking(other)) {
            activeCollisions.Add(other);
            UpdateMaterials();
        }
    }

    void OnTriggerExit(Collider other) {
        if (IsBlocking(other)) {
            activeCollisions.Remove(other);
            UpdateMaterials();
        }
    }

    bool IsBlocking(Collider other) {
        return other.GetComponentInParent<DynamicUnit>() || other.GetComponentInParent<StaticUnit>();
    }

    // Paint the ghost green if its position is valid, red otherwise
    void UpdateMaterials() {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
            renderer.sharedMaterial = Valid ? validMaterial : invalidMaterial;
        }
    }

    void Place() {
        if (Valid) {
            resources.SpendPopulation(populationCost);
            resources.SpendScrap(scrapCost);
            resources.SpendSpikes(spikesCost);
            resources.SpendCrawlbits(crawlbitsCost);

            GameObject building = Instantiate(buildingPrefab);
            building.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    public void Cancel() {
        Destroy(gameObject);
    }
}
