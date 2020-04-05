using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlacingBuilding : MonoBehaviour
{
    public Material validMaterial;
    public Material invalidMaterial;
    public GameObject buildingPrefab;
    public int populationCost;
    public int scrapCost;
    public int spikesCost;
    public int crawlbitsCost;

    const float MAX_HEIGHT = 11;
    const float MIN_HEIGHT = 3;

    public bool Valid {
        get { return Affordable && activeCollisions.Count == 0 && transform.position.y < MAX_HEIGHT && transform.position.y > MIN_HEIGHT; }
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

    private List<GameObject> spheres = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        terrainCollider = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        resources = GameObject.Find("Game Control").GetComponent<PlayerResources>();

        spheres.Add(GameObject.Find("Sphere"));
        spheres.Add(GameObject.Find("Sphere (1)"));
        spheres.Add(GameObject.Find("Sphere (2)"));
        spheres.Add(GameObject.Find("Sphere (3)"));
        spheres.Add(GameObject.Find("Sphere (4)"));
        spheres.Add(GameObject.Find("Sphere (5)"));
        spheres.Add(GameObject.Find("Sphere (6)"));
        spheres.Add(GameObject.Find("Sphere (7)"));
        spheres.Add(GameObject.Find("Sphere (8)"));
        spheres.Add(GameObject.Find("Sphere (9)"));
        spheres.Add(GameObject.Find("Sphere (10)"));
        spheres.Add(GameObject.Find("Sphere (11)"));
        spheres.Add(GameObject.Find("Sphere (12)"));

        UpdateMaterials();
    }

    // Update is called once per frame
    void Update() {

        if (GameObject.Find("AlienMothership") == null) // || GameObject.Find("HQ") == null)
        {
            return;
        }

        // Position the ghost of the model
        if (cam.pixelRect.Contains(Input.mousePosition)) {

            Vector3 buildingSize = Vector3.Scale(transform.localScale, GetComponent<Collider>().bounds.size);

            Vector3 yVec, xVec, zVec;
            float spawnRad = 15.0f;
            if (gameObject.tag == "TrainingCamp") 
            {
                //Add a bit of an extra radius around the training camp to make sure there is space for spawning
                yVec = new Vector3(0, 150, 0);
                xVec = new Vector3(buildingSize.x / 2 + spawnRad, 0, 0);
                zVec = new Vector3(0, 0, buildingSize.z / 2 + spawnRad);
            }
            else
            {
                yVec = new Vector3(0, 150, 0);
                xVec = new Vector3(buildingSize.x / 2, 0, 0);
                zVec = new Vector3(0, 0, buildingSize.z / 2);
            }
            
            List<Vector3> collisionPoints = new List<Vector3>();
            collisionPoints.Add(transform.position + yVec);
            collisionPoints.Add(transform.position + yVec + xVec);
            collisionPoints.Add(transform.position + yVec - xVec);
            collisionPoints.Add(transform.position + yVec + zVec);
            collisionPoints.Add(transform.position + yVec - zVec);
            collisionPoints.Add(transform.position + yVec + xVec / 2 + zVec / 2);
            collisionPoints.Add(transform.position + yVec + xVec / 2 - zVec / 2);
            collisionPoints.Add(transform.position + yVec - xVec / 2 + zVec / 2);
            collisionPoints.Add(transform.position + yVec - xVec / 2 - zVec / 2);
            collisionPoints.Add(transform.position + yVec + xVec / 2);
            collisionPoints.Add(transform.position + yVec - xVec / 2);
            collisionPoints.Add(transform.position + yVec + zVec / 2);
            collisionPoints.Add(transform.position + yVec - zVec / 2);

            float maxHeight = 0;
            float minHeight = 999999;
            int count = 0;
            foreach (Vector3 point in collisionPoints)
            {
                RaycastHit pointHit;
                Ray pointRay = new Ray(point, Vector3.down);

                if (terrainCollider.Raycast(pointRay, out pointHit, 1000.0f))
                {
                    spheres[count].transform.position = pointHit.point;
                    count++;
                    if (pointHit.point.y > maxHeight)
                    {
                        maxHeight = pointHit.point.y;
                    }
                    if (pointHit.point.y < minHeight)
                    {
                        minHeight = pointHit.point.y;
                    }
                }
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            terrainCollider.Raycast(ray, out RaycastHit hit, 1000.0f);

            if (maxHeight < MAX_HEIGHT && minHeight < MIN_HEIGHT)
            {
                transform.position = new Vector3(hit.point.x, minHeight, hit.point.z);
            }
            else
            {
                transform.position = new Vector3(hit.point.x, maxHeight, hit.point.z);
            }
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

        UpdateMaterials();
    }

    void OnTriggerEnter(Collider other) {
        if (IsBlocking(other)) {
            activeCollisions.Add(other);
        }
    }

    void OnTriggerExit(Collider other) {
        if (IsBlocking(other)) {
            activeCollisions.Remove(other);
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

            foreach(GameObject sphere in spheres)
            {
                sphere.transform.position = new Vector3(0, 9999999, 0);
            }
        }
        //See why it was not Valid 
        else
        {
            if (!Affordable)
            {
                GameObject panel = GameObject.Find("Canvas").transform.Find("Lower Panel").transform.Find("Resource Panel").gameObject;
                panel.GetComponent<ResourcePanel>().showInsufficiency(populationCost, scrapCost, spikesCost, crawlbitsCost);
            }
        }
    }

    public void Cancel() {
        Destroy(gameObject);

        foreach (GameObject sphere in spheres)
        {
            sphere.transform.position = new Vector3(0, 9999999, 0);
        }
    }
}
