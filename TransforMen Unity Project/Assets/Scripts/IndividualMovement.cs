using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualMovement : MonoBehaviour
{
    public Vector3 destination;
    public bool moving;
    public System.Action actionOnArrival;

    private float movementForce = 30.0f;
    private float separationForce = 50.0f;
    private float separationMinDistance = 30.0f;
    private float cohesionForce = 10.0f;
    private float cohesionMaxDistance = 80.0f;
    private float maxSpeed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        actionOnArrival = null;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update() {
        Rigidbody unitRB = GetComponent<Rigidbody>();

        DynamicUnit[] otherUnits = FindObjectsOfType<DynamicUnit>();
        Vector3 separationVec = new Vector3();
        Vector3 cohesionVec = new Vector3();
        foreach (DynamicUnit other in otherUnits) {
            if (other == null || other == GetComponent<DynamicUnit>()) {
                continue;
            }

            Vector3 dirToOther = (other.transform.position - transform.position);
            dirToOther.y = 0;
            if (dirToOther.magnitude < separationMinDistance) {
                separationVec += -dirToOther;
            } else if (dirToOther.magnitude < cohesionMaxDistance) {
                cohesionVec += dirToOther;
            }
        }

        if (separationVec.magnitude > 1) {
            separationVec = separationVec.normalized;
        }
        if (cohesionVec.magnitude > 1) {
            cohesionVec = cohesionVec.normalized;
        }


        if (moving) {
            Vector3 dir = (destination - transform.position).normalized;
            unitRB.AddForce(dir * movementForce * Time.deltaTime, ForceMode.VelocityChange);
            unitRB.AddForce(cohesionVec * cohesionForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        unitRB.AddForce(separationVec * separationForce * Time.deltaTime, ForceMode.VelocityChange);

        if (unitRB.velocity.magnitude > maxSpeed) {
            unitRB.velocity = unitRB.velocity.normalized * maxSpeed;
        }

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


