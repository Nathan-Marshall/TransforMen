using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualMovement : MonoBehaviour
{
    public Vector3 destination;
    public Transform initialTransform;
    public Vector3 initialPosition;

    IEnumerator moveRoutine = null;

    public bool toMove;
    public System.Action actionOnArrival;
    private float speed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        toMove = false;
        actionOnArrival = null;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //If we have received a command to move, and haven't yet done so, do that this frame
        if (toMove)
        {
            Rigidbody unitRB = GetComponent<Rigidbody>();

            Vector3 dir = (destination - transform.position).normalized;

            // Start animation
            initialTransform = transform;
            initialPosition = transform.position;

            if (moveRoutine == null)
            {
                moveRoutine = Move();
                StartCoroutine(moveRoutine);
            }
            else
            {
                StopCoroutine(moveRoutine);
                moveRoutine = Move();
                StartCoroutine(moveRoutine);
            }

            toMove = false;
        }
    }

    Vector3 InterpPosition(float t)
    {
        if (t < 0.0f) { t = 0.0f; }
        if (t > 1.0f) { t = 1.0f; }

        float tlen = t * Vector3.Distance(initialPosition, destination);

        if (tlen > Vector3.Distance(initialPosition, destination)){
            return destination;
        }

        float s2 = Mathf.Pow(t, 2);
        float s3 = Mathf.Pow(t, 3);
        Vector3 pos = (2 * s3 - 3 * s2 + 1) * initialPosition +
                      (s3 - 2 * s2 + t) * new Vector3(0,0,-1) +
                      (-2 * s3 + 3 * s2) * destination +
                      (s3 - s2) * new Vector3(0,0,-1);
        return pos;
    }

    IEnumerator Move()
    {
        for (float s = 0.0f; s < 1.0f; s += 0.01f)
        {
            Vector3 newPos = InterpPosition(s);
            this.transform.position = new Vector3(newPos.x, this.transform.position.y, newPos.z);

            Vector3 curve_tan = InterpPosition(s + 0.01f) - InterpPosition(s);
            curve_tan.Normalize();

            if (s >= 0.99f)
            {
                curve_tan = new Vector3(0, 0, -1); ;
            }


            Quaternion orient = new Quaternion();
            orient.SetLookRotation(new Vector3(curve_tan.x, 0, curve_tan.z), Vector3.up);
            orient *= Quaternion.AngleAxis(180, Vector3.forward);
            orient *= Quaternion.AngleAxis(180, Vector3.right);
            orient *= Quaternion.AngleAxis(270, Vector3.up);

            this.transform.rotation = orient;
            yield return new WaitForSeconds(0.05f);
        }
        this.transform.position = destination;
        this.transform.position = new Vector3(destination.x, this.transform.position.y, destination.z);

        if (actionOnArrival != null)
        {
            actionOnArrival();
            actionOnArrival = null;
        }

        destination = transform.position;

        yield return null;
    }
}