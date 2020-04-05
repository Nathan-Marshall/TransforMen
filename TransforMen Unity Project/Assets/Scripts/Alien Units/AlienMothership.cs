using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Alien Mothership: A static unit. Player wins if this destroyed
//     Spawns alien units. 
//-------------------------------------------------------------

public class AlienMothership : StaticUnit
{
    //Collaborators: Static Unit, Attack Target 

    //Human win condition
    //can spawn enemies 

    // Start is called before the first frame update
    void Start()
    {
        SetToAlienTeam();
        SetSelectable(false);

        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.Enemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<AttackTarget>().GetHealth() <= 0)
        { 
            StartCoroutine(VictoryEnd());
        }
        
    }

    IEnumerator VictoryEnd()
    {
        yield return new WaitForSeconds(1f);

        GameObject.Find("Canvas").transform.Find("End Screen").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("End Screen").GetComponent<TMPro.TextMeshProUGUI>().SetText("VICTORY!!");

        GameObject[] allObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject child in allObjs) 
        {
            if (child.tag.Equals("Enemy"))
            {
                //Remove all resources so that when they die, you supply doesnt skyrocket
                child.GetComponent<AttackTarget>().SetSpikeResource(0);
                child.GetComponent<AttackTarget>().SetCrawlResource(0);
                child.GetComponent<AttackTarget>().SetScrapResource(0);

                //Kill all existing enemies 
                child.GetComponent<AttackTarget>().TakeDamage(10000);
            }
        }
    }
}
