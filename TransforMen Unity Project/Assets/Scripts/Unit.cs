using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Unit: a generic unit
//-------------------------------------------------------------

public class Unit : MonoBehaviour
{
    //Collaborators: Static Unit, Dynamic Unit, AttackUnit

    protected Vector3 position; //position in the world 
    //Model 
    public enum Team { Player, Enemy }; //the two possible teams one can be on 
    protected Team team; //whether the unit belongs to Player or the enemy 
    protected bool selectable; //whether or not the unit can be selected 
    protected string description; //the info about the unit

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get & Set Team 
    public Team GetTeam() { return team; }
    protected void SetToPlayerTeam()
    {
        team = Team.Player;
    }
    protected void SetToAlienTeam()
    {
        team = Team.Enemy;
    }

    //Get & Set Selectability 
    public bool GetSelectable() { return selectable; }
    public void SetSelectable(bool newSelectable)
    {
        selectable = newSelectable;
    }

    //Get & Set Description
    public string GetDescription() { return description; }
    public void SetDescription(string newDescription)
    {
        description = newDescription;
    }
    
}
