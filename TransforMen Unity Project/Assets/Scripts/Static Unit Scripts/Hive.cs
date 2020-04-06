using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : StaticUnit
{
    public GameObject spawnedEnemy;
    public float spawnRate;
    public EnemyAI.EnemyState initialState;
    public int maxSpawnNum;

    private float spawnDist = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.Enemy);

        if (spawnedEnemy == null)
        {
            Debug.LogError("ERROR: Spawned enemy is null!");
        }
        else
        {
            string enemyType = spawnedEnemy.name;

            if (enemyType.Equals("SpikeAlien"))
            {
                int spikeResource = spawnedEnemy.GetComponent<SpikeAlien>().GetComponent<AttackTarget>().GetSpikeResource(); 
                GetComponent<AttackTarget>().SetSpikeResource(spikeResource); 
            }
            else if (enemyType.Equals("CrawlerAlien"))
            {
                int crawlResource = spawnedEnemy.GetComponent<AlienCrawlerMother>().GetComponent<AttackTarget>().GetCrawlResource();
                GetComponent<AttackTarget>().SetCrawlResource(crawlResource);
            }
            else
            {
                Debug.LogError("ERROR: Spawned Enemy not recognized!");
            }

        }

        //Debug.Log(GetComponent<AttackTarget>().); 

        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator Spawn()
    {
        Transform spawnStart = transform.Find("SpawnPoint");
        
        while (true)
        {
            Vector3 forceDir = (spawnStart.position - transform.position).normalized;
            Vector3 forceModifier = Vector3.Cross(forceDir, Vector3.up).normalized;

            float modifierStrength = Random.Range(0.0f, 2.0f);
            if (Random.value > 0.5)
            {
                modifierStrength *= -1;
            }

            Vector3 spawnPos = transform.position + (forceDir * 5) + (forceModifier * modifierStrength);


            bool canSpawn = true;
            if (initialState == EnemyAI.EnemyState.DEFENDING)
            {
                GameObject[] aliens = GameObject.FindGameObjectsWithTag("Enemy");
                List<GameObject> defendingAliens = new List<GameObject>();

                for (int i = 0; i < aliens.Length; i++)
                {
                    if (Vector3.Distance(aliens[i].transform.position, transform.position) <= EnemyAI.DEFEND_RADIUS && aliens[i] != this.gameObject)
                    {
                        defendingAliens.Add(aliens[i]);
                    }
                }

                if (defendingAliens.Count >= maxSpawnNum)
                {
                    canSpawn = false;
                }
            }

            if (canSpawn)
            {
                GameObject newSpawn = Instantiate(spawnedEnemy, spawnPos, Quaternion.identity);

                yield return new WaitForSeconds(0.05f);
                newSpawn.GetComponent<EnemyAI>().ChangeEnemyState(initialState);
            } 

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
