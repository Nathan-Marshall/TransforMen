using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Resource: Can be harvested for a resource by human infantry
//-------------------------------------------------------------

public class Resource : MonoBehaviour
{
    //Collaborators: Static Unit 

    //I feel like maybe this class doesn't currently make much sense? 
    enum ResourceType { Human, Metal }; //kind of resources that can be found 
    int quantity;
    bool canBeHarvested; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
