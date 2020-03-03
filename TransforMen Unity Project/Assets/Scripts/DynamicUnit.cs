using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Dynamic Unit: a unit who can move on the map
//-------------------------------------------------------------

public class DynamicUnit : Unit
{
    //Collaborators: Unit, Static Unit, Dynamic Unit (others), Attack Targets 

    bool transportable; //whether or not the unit can be transported via vehicle 
    float moveSpeed; //how quickly the unit travels 
    bool canScavenge; //whether or not the unit can scavenge
    bool controllable; //whether or not the unit can be directly controlled 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
