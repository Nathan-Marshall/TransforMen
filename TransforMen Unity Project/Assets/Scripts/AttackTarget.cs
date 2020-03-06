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
    public int health; //the amount of health this target has 
    public int defense; //the defense of this target 

    // Start is called before the first frame update
    void Start() {
        GetComponent<BehaviourMap>().targetTypes.Add(UnitController.TargetType.Enemy);
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

    public void TakeDamage(int baseDamage) {
        int finalDamage = Mathf.Max(1, (int)(baseDamage * 0.1f + Mathf.Max(0, baseDamage - defense) * 0.9f));
        health -= finalDamage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        Destroy(gameObject);
    }
}
