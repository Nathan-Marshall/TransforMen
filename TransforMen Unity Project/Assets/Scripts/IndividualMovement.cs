using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualMovement : MonoBehaviour
{
    public Vector3 destination;
    public bool moving;
    
    private float speed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
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
        }
    }
}


