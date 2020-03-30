using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : StaticUnit
{
    public GameObject spawnedEnemy;
    public float spawnRate;
    private float spawnDist = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.Enemy);

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

            GameObject newSpawn = Instantiate(spawnedEnemy, spawnPos, Quaternion.identity);

            newSpawn.GetComponent<Rigidbody>().AddForce((spawnPos - transform.position).normalized * 10000, ForceMode.Impulse);

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
