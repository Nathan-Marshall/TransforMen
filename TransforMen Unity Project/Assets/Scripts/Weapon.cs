using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Weapon: used by attack unit to attack
//-------------------------------------------------------------

public class Weapon
{
    //Collaborators: Unit, Attack Unit, Projectile 

    protected float range; //the range the weapon can reach
    protected float speed; //the speed at which the weapon fires/attacks
    protected int damage; //the damage the weapon has
    protected float firingRate; //the rate at which projectiles are fired 
    //Attack effects
    //Projectile Type 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get & Set Range
    public float GetRange() { return range; }
    protected void SetRange(float newRange )
    {
        range = newRange;
    }

    //Get & Set Speed
    public float GetSpeed() { return speed; }
    protected void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    //Get & Set Damage 
    public int GetDamage() { return damage; }
    protected void SetDamage(int newDamage )
    {
        damage = newDamage;
    }

    //Get & Set Firing Rate 
    public float GetFiringRate() { return firingRate; }
    protected void SetFiringRate(float newFireRate)
    {
        firingRate = newFireRate;
    }
}
