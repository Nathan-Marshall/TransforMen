﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
// Attack Target: units with this component can be attacked and 
//                serve as targets for attack units 
//-------------------------------------------------------------

public class AttackTarget : MonoBehaviour
{
    //Collaborators: Attack Unit, Dynamic Unit 

    private Animator animator; 

    //On damaged event 
    public int health; //the amount of health this target has 
    public int defense; //the defense of this target 
    public int spikeResource; //how many spikes this target gives to the player 
    public int crawlResource; //how many crawl bits this target gives to the player 
    public int scrapResource; //how much scrap this target gives to player 

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetInteger("Health", health);
        }
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

    //Get Resources
    public int GetSpikeResource() { return spikeResource; }
    public void SetSpikeResource(int newSpike)
    {
        spikeResource = newSpike; 
    }
    public int GetCrawlResource() { return crawlResource; }
    public void SetCrawlResource(int newCrawl)
    {
        crawlResource = newCrawl;
    }
    public int GetScrapResource() { return scrapResource; }
    public void SetScrapResource(int newScrap)
    {
        scrapResource = newScrap;
    }

    public void TakeDamage(int baseDamage) {
        int finalDamage = Mathf.Max(1, (int)(baseDamage * 0.1f + Mathf.Max(0, baseDamage - defense) * 0.9f));
        health -= finalDamage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {

        StartCoroutine(AnimatedDeath());

        GameObject controller = GameObject.Find("Game Control");
        PlayerResources resourceControl = controller.GetComponent<PlayerResources>();

        resourceControl.AddSpikes(spikeResource); 
        resourceControl.AddCrawlbits(crawlResource);
        resourceControl.AddScrap(scrapResource); 
    }

    IEnumerator AnimatedDeath() {
        IndividualMovement movement = GetComponent<IndividualMovement>();
        if (movement != null) {
            movement.CancelMovement();
        }

        if (animator)
        {
            animator.SetInteger("Health", health);
            yield return new WaitForSeconds(1.5f);
        }

        //Check if we have a crawler mother, we will have to spawn units upon death
        if (gameObject.GetComponent<AlienCrawlerMother>() != null)
        {
            gameObject.GetComponent<AlienCrawlerMother>().SpawnChildren();
        }

        Destroy(gameObject);
        yield return null;
    }
}
