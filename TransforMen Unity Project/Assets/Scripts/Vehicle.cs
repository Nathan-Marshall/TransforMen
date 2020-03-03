using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Vehicle: dyanmic unit capable of transporting other dynamic
//    units at a high speed. Can be attacked an destroyed by 
//    enemy. On death, will drop scrap metal which can be 
//    scavenged by infantry units. 
//-------------------------------------------------------------

public class Vehicle : DynamicUnit
{
    //Collaborators: Dynamic Unit

    List<DynamicUnit> heldUnits; //units currently being carried 
    int capacity; //number of units that can be held

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get & Set Capacity 
    public int GetCapacity() { return capacity; }
    protected void SetCapacity(int newCapacity)
    {
        capacity = newCapacity;
    }
}
