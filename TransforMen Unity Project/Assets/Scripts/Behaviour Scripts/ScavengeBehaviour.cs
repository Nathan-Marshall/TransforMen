using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeBehaviour : MonoBehaviour, UnitAction
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        BehaviourMap mapping = GetComponent<BehaviourMap>();
        mapping.behaviourMap.Add(UnitController.TargetType.Ruin, GetType());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Scavenge(GameObject target)
    {
        if (target == null) {
            return;
        }

        StartCoroutine(Animation(target));

        GameObject controller = GameObject.Find("Game Control");
        PlayerResources resourceControl = controller.GetComponent<PlayerResources>();
        Ruin ruin = target.GetComponent<Ruin>();

        int pop = ruin.GetPopulation();
        int scrap = ruin.GetScrap();

        resourceControl.AddPopulation(pop);
        resourceControl.AddScrap(scrap);
    }


    public System.Action GetAction(GameObject target)
    {
        return (() => Scavenge(target));
    }

    IEnumerator Animation(GameObject target)
    {
        PlayAnimation();

        yield return new WaitForSeconds(1.5f);

        StopAnimation();

        Destroy(target);
    }

    void PlayAnimation()
    {
        animator.SetBool("Scavenge", true);
        animator.SetFloat("Speed", 0); 
    }

    void StopAnimation()
    {
        animator.SetBool("Scavenge", false);
    }
}
