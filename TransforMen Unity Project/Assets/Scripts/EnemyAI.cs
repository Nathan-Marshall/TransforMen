using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    void Start()
    {
        MoveToRandomDestination();
    }

    void Update()
    {
        
    }

    void MoveToRandomDestination() {
        Destination destination = null;
        IndividualMovement movement = GetComponent<IndividualMovement>();
        while (destination == null || !movement.DestinationReachable(destination)) {
            destination = new Destination(new Vector3(Random.Range(-500, 500), 0, Random.Range(-500, 500)));
        }
        movement.MoveTo(destination, MoveToRandomDestination);
    }
}
