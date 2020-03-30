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
        Transform spawnStart = transform.Find("SpawnStart");
        Vector3 forceDir = new Vector3(0, 0, 1);

        while (true)
        {
            GameObject newSpawn = Instantiate(spawnedEnemy, spawnStart.position, Quaternion.identity);

            newSpawn.GetComponent<Rigidbody>().AddForce(forceDir * 10000, ForceMode.Impulse);

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
