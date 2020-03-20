using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualMovement : MonoBehaviour
{
    const float InteractionRange = 15.0f;

    private Animator animator;
    public Destination destination;

    public bool moving;

    private float movementForce = 30.0f;
    private float separationForce = 50.0f;
    private float separationMinDistance = 10.0f;
    private float cohesionForce = 10.0f;
    private float cohesionMaxDistance = 80.0f;
    private float maxSpeed = 40.0f;

    private Vector3 initialPosition;
    private IEnumerator moveRoutine = null;
    private System.Action actionOnArrival;

    // Start is called before the first frame update
    void Start() {
        actionOnArrival = null;
        destination = null;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Rigidbody unitRB = GetComponent<Rigidbody>();

        if (moving) {
            if (destination == null || !destination.Exists()) {
                ReachedDestination();
            } else {
                DynamicUnit[] otherUnits = FindObjectsOfType<DynamicUnit>();
                Vector3 separationVec = new Vector3();
                Vector3 cohesionVec = new Vector3();
                foreach (DynamicUnit other in otherUnits) {
                    if (other == null || other == GetComponent<DynamicUnit>()
                            || other.GetComponent<IndividualMovement>() == null
                            || !other.GetComponent<IndividualMovement>().moving) {
                        continue;
                    }

                    Vector3 dirToOther = (other.GetComponent<Collider>().ClosestPoint(transform.position)
                        - GetComponent<Collider>().ClosestPoint(other.transform.position));
                    dirToOther.y = 0;
                    if (dirToOther.magnitude < separationMinDistance) {
                        separationVec += -dirToOther;
                    }
                    else if (dirToOther.magnitude < cohesionMaxDistance) {
                        cohesionVec += dirToOther;
                    }
                }

                if (separationVec.magnitude > 1) {
                    separationVec = separationVec.normalized;
                }
                if (cohesionVec.magnitude > 1) {
                    cohesionVec = cohesionVec.normalized;
                }

                Vector3 dir = (destination.Position - transform.position).normalized;
                unitRB.AddForce(dir * movementForce * Time.deltaTime, ForceMode.VelocityChange);
                unitRB.AddForce(cohesionVec * cohesionForce * Time.deltaTime, ForceMode.VelocityChange);
                unitRB.AddForce(separationVec * separationForce * Time.deltaTime, ForceMode.VelocityChange);

                if (unitRB.velocity.magnitude > maxSpeed) {
                    unitRB.velocity = unitRB.velocity.normalized * maxSpeed;
                }

                dir.Normalize();

                Quaternion orient = new Quaternion();
                orient.SetLookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up);
                orient *= Quaternion.AngleAxis(180, Vector3.forward);
                orient *= Quaternion.AngleAxis(180, Vector3.right);
                orient *= Quaternion.AngleAxis(270, Vector3.up);

                this.transform.rotation = orient;

                Vector3 closestToDest = GetComponent<Collider>().ClosestPoint(destination.Position);
                closestToDest.y = 0;
                Vector3 closestToThis = destination.ClosestPoint(transform.position);
                closestToThis.y = 0;
                Debug.Log(this + " p:" + transform.position + " ctd:" + closestToDest + " d:" + destination.Position + " ctt:" + closestToThis);
                if ((closestToThis - closestToDest).magnitude < InteractionRange) {
                    ReachedDestination();
                }
            }
        } else {
            unitRB.velocity *= 0.99f;
        }
    }

    public void MoveTo(Destination dest, System.Action action, bool individual)
    {
        if (individual)
        {
            MoveToIndividually(dest, action);
        }
        else
        {
            MoveAnimation();

            destination = dest;
            actionOnArrival = action;
            moving = true;
        }
    }

    Vector3 InterpPosition(float t, Vector3 closestDestPoint) {
        if (t < 0.0f) { t = 0.0f; }
        if (t > 1.0f) { t = 1.0f; }

        float tlen = t * Vector3.Distance(initialPosition, closestDestPoint);

        if (tlen > Vector3.Distance(initialPosition, closestDestPoint)) {
            return closestDestPoint;
        }

        //Calculate the position along the spline.
        //This should take into account easing as well
        float s2 = Mathf.Pow(t, 2);
        float s3 = Mathf.Pow(t, 3);
        Vector3 pos = (2 * s3 - 3 * s2 + 1) * initialPosition +
                      (s3 - 2 * s2 + t) * new Vector3(0, 0, -1) +
                      (-2 * s3 + 3 * s2) * closestDestPoint +
                      (s3 - s2) * new Vector3(0, 0, -1);
        return pos;
    }

    IEnumerator Move() {

        Vector3 closestDestPoint = destination.ClosestPoint(transform.position);

        float total_dist = Vector3.Distance(initialPosition, closestDestPoint);

        float increment_factor = total_dist / 50;

        for (float s = 0.0f; s < 1.0f; s += 0.01f / increment_factor) {
            Vector3 newPos = InterpPosition(s, closestDestPoint);
            this.transform.position = new Vector3(newPos.x, this.transform.position.y, newPos.z);

            Vector3 curve_tan = InterpPosition(s + 0.01f, closestDestPoint) - InterpPosition(s, closestDestPoint);
            curve_tan.Normalize();

            if (s >= 0.99f) {
                curve_tan = new Vector3(0, 0, -1);
            }

            Quaternion orient = new Quaternion();
            orient.SetLookRotation(new Vector3(curve_tan.x, 0, curve_tan.z), Vector3.up);
            orient *= Quaternion.AngleAxis(180, Vector3.forward);
            orient *= Quaternion.AngleAxis(180, Vector3.right);
            orient *= Quaternion.AngleAxis(270, Vector3.up);

            this.transform.rotation = orient;
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = new Vector3(closestDestPoint.x, transform.position.y, closestDestPoint.z);

        ReachedDestination();

        yield return null;
    }


    public void MoveToIndividually(Destination dest, System.Action action) {
        MoveAnimation();

        if (moveRoutine != null) {
            StopCoroutine(moveRoutine);
        }

        actionOnArrival = action;
        destination = dest;

        initialPosition = transform.position;

        moveRoutine = Move();
        StartCoroutine(moveRoutine);
    }

    void MoveAnimation() {
        animator.SetFloat("Speed", 1f);
    }

    void StopAnimation() {
        animator.SetFloat("Speed", 0f);
    }

    private void ReachedDestination() {
        destination = null;
        moving = false;
        GetComponent<Rigidbody>().velocity *= 0.1f;

        if (actionOnArrival != null) {
            actionOnArrival();
            actionOnArrival = null;
        }

        StopAnimation();
    }
}