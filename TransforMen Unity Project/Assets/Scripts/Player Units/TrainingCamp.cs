using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Training Camp: which can train human infantry, and consumes
//      population/human resource. Can be attacked and destroyed 
//      by enemies
//-------------------------------------------------------------

public class TrainingCamp : StaticUnit
{
    //Collaborators: Static Unit, Attack Target 
    private string desc = "Training camp to train average people to soldiers";
    //COST: 5 scrap
    //Can train infantry 
    public GameObject infantryPrefab; 
    //Consumes resources 
    public int populationCost;
    public int scrapCost;
    private PlayerResources resources;
    private Collider boxCollider;

    private int queueLength = 0;
    private float currentTrainTime = 0.0f;

    const float TRAINING_TIME = 5.0f;


    public bool Affordable
    {
        get
        {
            return resources.GetPopulationResource() >= populationCost
                    && resources.GetScrapResource() >= scrapCost;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(true);
        SetDescription(desc);

        resources = GameObject.Find("Game Control").GetComponent<PlayerResources>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Train()
    {
        //If the building is clicked and can afford, make a new unit 
        if (Affordable)
        {
            //Spawn a guy
            resources.SpendPopulation(populationCost);
            resources.SpendScrap(scrapCost);

            if (queueLength == 0 && currentTrainTime == 0.0f)
            {
                currentTrainTime = TRAINING_TIME;
            }

            queueLength++;
        }
        else if (!Affordable)
        {
            GameObject panel = GameObject.Find("Canvas").transform.Find("Lower Panel").transform.Find("Resource Panel").gameObject;
            panel.GetComponent<ResourcePanel>().showInsufficiency(populationCost, scrapCost, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

        if (boxCollider.Raycast(ray, out RaycastHit hit, 10000.0f) && Input.GetMouseButtonDown(0))
        {
            GameObject.Find("Game Control").GetComponent<PanelControl>().SetInfo(
                "Training Queue Length:", () => { return queueLength; },
                "Current Training Time:", () => { return currentTrainTime; },
                "Cost:", string.Format("{0} Population\n{1} Scrap", populationCost, scrapCost),
                "Train new infantry at the training camp.\n\nInfantry are basic units which can fire at enemies from range, and also have scavenging capabilities",
                "Train New Infantry", () => Train());
        }



        if (currentTrainTime > 0)
        {
            currentTrainTime -= Time.deltaTime;
            if (currentTrainTime < 0)
            {
                currentTrainTime = 0;
            }
        }

        if (queueLength > 0)
        {
            if (currentTrainTime == 0.0)
            {
                MakeInfantry();
                queueLength--;

                if (queueLength > 0)
                {
                    currentTrainTime = TRAINING_TIME;
                }
            } 
        }
    }

    void MakeInfantry()
    {
        Vector3 buildingSize = Vector3.Scale(transform.localScale, GetComponent<Collider>().bounds.size);

        //Y value of 150 is about as high as it goes
        Vector3 yVec = new Vector3(0, 150, 0);
        Vector3 xVec = new Vector3(buildingSize.x / 2, 0, 0);
        Vector3 zVec = new Vector3(0, 0, buildingSize.z / 2);

        GameObject infantry = Instantiate(infantryPrefab, transform.position - zVec, Quaternion.identity);
    }
}
