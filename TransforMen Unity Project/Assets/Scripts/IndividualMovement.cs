using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualMovement : MonoBehaviour
{
    private Animator animator; 
    public Vector3 destination;

    private Vector3 initialPosition;
    private IEnumerator moveRoutine = null;
    private System.Action actionOnArrival;

    // Start is called before the first frame update
    void Start()
    {
        actionOnArrival = null;
        destination = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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

        this.transform.position = new Vector3(destination.x, this.transform.position.y, destination.z);

        StopAnimation();

        if (actionOnArrival != null)
        {
            actionOnArrival();
            actionOnArrival = null;
        }

        destination = transform.position;

        yield return null;
    }


    public void MoveTo(Vector3 dest, System.Action action)
    {
        MoveAnimation();

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        actionOnArrival = action;
        destination = dest;

        initialPosition = transform.position;

        moveRoutine = Move();
        StartCoroutine(moveRoutine);
    }

    void MoveAnimation()
    {
        animator.SetFloat("Speed", 1f);
    }

    void StopAnimation()
    {
        animator.SetFloat("Speed", 0f);
    }
}