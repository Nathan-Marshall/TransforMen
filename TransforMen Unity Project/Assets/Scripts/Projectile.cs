using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Projectile: are fired by weapons and damage attack targets,
//             interacts with attack targets 
//-------------------------------------------------------------

public class Projectile : MonoBehaviour
{
    //Collaborators: Attack Target, Attacker, Weapon 

    protected Vector3 position; //position of the projectile 
    protected int speed; //speed the projectile travels, given by weapon which fired it 
    protected int damage; //damage the projectile causes, given by weapon which fired it
    protected AttackTarget target; //target which the projectile is heading towards 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get & Set Position


    //Get & Set Speed 

    //Get & Set Damage 

    //Get & Set Target 
}
