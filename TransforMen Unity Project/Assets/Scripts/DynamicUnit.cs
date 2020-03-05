using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Dynamic Unit: a unit who can move on the map
//-------------------------------------------------------------

public class DynamicUnit : Unit
{
    //Collaborators: Unit, Static Unit, Dynamic Unit (others), Attack Targets 

    protected bool transportable; //whether or not the unit can be transported via vehicle 
    protected float moveSpeed; //how quickly the unit travels 
    protected bool canScavenge; //whether or not the unit can scavenge
    protected bool controllable; //whether or not the unit can be directly controlled 

    public bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print("Here");
        if (GameObject.Find("Game Control").GetComponent<UnitSelect>().selectedUnits.Contains(gameObject))
        {
            gameObject.transform.Find("SelProjector").GetComponent<Projector>().enabled = true;
        }
        else
        {
            gameObject.transform.Find("SelProjector").GetComponent<Projector>().enabled = false;
        }
    }

    //Get & Set Transportable
    public bool GetTransportable() { return transportable; }
    protected void SetTransportable(bool ifTransportable)
    {
        transportable = ifTransportable;
    }

    //Get & Set MoveSpeed
    public float GetMoveSpeed() { return moveSpeed; }
    protected void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    //Get & Set CanScavenge
    public bool GetCanScavenge() { return canScavenge; }
    protected void SetCanScavenge(bool ifScavenge)
    {
        canScavenge = ifScavenge;
    }

    //Get & Set Controllable 
    public bool GetControllable() { return controllable; }
    protected void SetControllable(bool ifControllable)
    {
        controllable = ifControllable;
    }
}
