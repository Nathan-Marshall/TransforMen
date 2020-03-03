using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Attack Target: units with this component can be attacked and 
//                serve as targets for attack units 
//-------------------------------------------------------------

public class AttackTarget : MonoBehaviour
{
    //Collaborators: Attack Unit, Dynamic Unit 

    //On damaged event 
    protected int health; //the amount of health this target has 
    protected int defense; //the defense of this target 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get & Set Health
    public int GetHealth() { return health; }
    protected void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    //Get & Set Defense 
    public int GetDefense() { return defense; }
    protected void SetDefense(int newDefense)
    {
        defense = newDefense;
    }
}
