using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Alien Crawler Mother: dynamic unit that can attack human targets. 
//    is a simple melee unit that deals moderate damage 
//-------------------------------------------------------------

public class AlienCrawlerMother : AttackUnit
{
    private string desc = "Alien who spawns enemies";
    //Collaborators: Dynamic Unit, Attack Target, Attack Unit 

    //AI-Controlled

    float steerVelocity;
    bool moves;

    public GameObject childPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SetToAlienTeam();
        SetSelectable(false);
        SetDescription(desc);
        SetTransportable(true);
        SetMoveSpeed(30);
        SetCanScavenge(false);
        SetControllable(false);
        SetCanAttack(true);
        SetWeapon(new Melee());
        SetAttackTarget(null);

        steerVelocity = Random.value * 0.5f - 0.25f;
        transform.Rotate(0, Random.value * 360, 0);
        moves = Random.value > 0.3f;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moves)
        {
            animator.SetFloat("Speed", 1.0f);
        }

        if (moves && GetComponent<AttackTarget>().GetHealth() > 0) {
            Rigidbody rb = GetComponent<Rigidbody>();
            transform.Rotate(0, steerVelocity, 0);
            rb.velocity = transform.rotation * new Vector3(-moveSpeed, 0, 0);
        }
    }

    public void SpawnChildren()
    {
        Vector3 motherPos = this.transform.position;

        Vector3 p1, p2, p3, p4;

        p1 = motherPos + new Vector3(5, 0, 0);
        p2 = motherPos + new Vector3(-5, 0, 0);
        p3 = motherPos + new Vector3(0, 0, 5);
        p4 = motherPos + new Vector3(0, 0, -5);

        Instantiate(childPrefab, p1, Quaternion.identity);
        Instantiate(childPrefab, p2, Quaternion.identity);
        Instantiate(childPrefab, p3, Quaternion.identity);
        Instantiate(childPrefab, p4, Quaternion.identity);
    }
}
