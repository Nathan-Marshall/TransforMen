using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IndividualMovement : MonoBehaviour
{
    public const float InteractionRange = 15.0f;
    public const float UpdateDestinationRange = 10.0f;

    public bool moving;
    public bool attackMovement;

    private Animator animator;
    private Destination destination;
    private System.Action actionOnArrival;

    // Start is called before the first frame update
    void Start() {
        actionOnArrival = null;
        destination = null;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (moving) {
            if (destination == null || !destination.Exists()) {
                ReachedDestination();
            } else {
                Vector3 closestToDest = GetComponent<Collider>().ClosestPoint(destination.Position);
                Vector3 closestToThis = destination.ClosestPoint(transform.position);

                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                if ((agent.destination - closestToThis).magnitude > UpdateDestinationRange) {
                    agent.destination = closestToThis;
                }

                closestToDest.y = 0;
                closestToThis.y = 0;

                float targetRange = attackMovement ? GetComponent<AttackUnit>().GetWeapon().GetRange() : InteractionRange;
                if ((closestToThis - closestToDest).magnitude < targetRange) {
                    ReachedDestination();
                }
            }
        }
    }

    public void MoveTo(Destination dest, System.Action action, bool isAttacking = false) {
        // Don't do anything if there's no path to the destination
        if (DestinationReachable(dest)) {
            moving = true;
            attackMovement = isAttacking;

            MoveAnimation();

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = dest.ClosestPoint(transform.position);
            agent.isStopped = false;
            destination = dest;
            actionOnArrival = action;
        }
    }

    void MoveAnimation() {
        animator.SetFloat("Speed", 1f);
    }

    void StopAnimation() {
        animator.SetFloat("Speed", 0f);
    }

    public bool DestinationReachable(Destination dest) {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 destinationPoint = dest.ClosestPoint(transform.position);
        NavMeshPath path = new NavMeshPath();
        return agent.CalculatePath(destinationPoint, path) && path.status == NavMeshPathStatus.PathComplete;
    }

    private void ReachedDestination() {
        System.Action tempAction = actionOnArrival;
        CancelMovement();
        tempAction?.Invoke();
    }

    public void CancelMovement() {
        destination = null;
        moving = false;
        GetComponent<NavMeshAgent>().isStopped = true;
        StopAnimation();
        actionOnArrival = null;
    }
}