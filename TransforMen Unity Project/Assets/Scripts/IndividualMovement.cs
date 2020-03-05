using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualMovement : MonoBehaviour
{
    public Vector3 destination;
    public bool moving;
    public System.Action actionOnArrival;
    private float speed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        actionOnArrival = null;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Rigidbody unitRB = GetComponent<Rigidbody>();

            Vector3 dir = (destination - transform.position).normalized;

            unitRB.MovePosition(transform.position + dir * speed * Time.deltaTime);

            if ((GetComponent<Collider>().ClosestPointOnBounds(destination) - destination).magnitude == 0)
            {
                destination = transform.position;
                moving = false;

                if (actionOnArrival != null)
                {
                    actionOnArrival();
                    actionOnArrival = null;
                }
            }
        }
    }
}


