using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Resistance Base: A static unit. Aliens win if this destroyed
//-------------------------------------------------------------

public class ResistanceBase : StaticUnit
{
    //Collaborators: Static Unit, Attack Target 
    private string desc = "Headquarters of the human resistance";
    //Starts spawned on the map 

    //Alien Win Condition 

    // Start is called before the first frame update
    void Start()
    {
        SetToPlayerTeam();
        SetSelectable(true);
        SetDescription(desc);
        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.HQ);
    }

    // Update is called once per frame
    void Update()
    {
        //Missing right now because HQ is not currently an attack target 
        //if health is 0
        //      call DefeatEnd() 
    }

    IEnumerator DefeatEnd()
    {
        yield return new WaitForSeconds(1f);

        GameObject.Find("Canvas").transform.Find("End Screen").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("End Screen").GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Defeat...");
    }
}
