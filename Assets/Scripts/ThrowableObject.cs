using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : GrabbableObject
{
    public float shootForce;
    public bool offsetGrab;

    private Grabber tempHand;

    private FixedJoint joint;
    private Vector3 previousPosition;
    private Queue<Vector3> previousVelocities = new Queue<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnHoverStart()
    {
        // base.OnHoverStart();

    }
    public override void OnHoverEnd()
    {
        // base.OnHoverEnd();
    }
    public override void OnGrabStart(Grabber hand)
    {

        if (!offsetGrab)
        {
            transform.position = hand.transform.position;
            transform.rotation = hand.transform.rotation;
        }

        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = hand.GetComponent<Rigidbody>();

        // part of action to "shoot" from hand - no longer used
        // base.OnGrabStart(hand);
        // tempHand = hand;
    }
    public override void OnGrabEnd()
    {
        if (joint != null)
        {
            // Remove the joint
            Destroy(joint);

            // Calculate the velocity
            var averageVelocity = Vector3.zero;
            foreach (var velocity in previousVelocities)
            {
                averageVelocity += velocity;
            }

            averageVelocity /= previousVelocities.Count;

            // Apply the velocity (throw)
            GetComponent<Rigidbody>().velocity = averageVelocity * shootForce;
        }

        // Propel object (shoot) - no longer used
        // base.OnGrabEnd();
        // GetComponent<Rigidbody>().AddForce(tempHand.transform.forward * shootForce);
        // tempHand = null;
    }

    private void FixedUpdate()
    {
        // Calculate a single velocity
        var velocity = transform.position - previousPosition;

        // Remember the current position to be used in the next iteration
        previousPosition = transform.position;

        // Add the calculated velocity into our queue
        previousVelocities.Enqueue(velocity);

        // If the size of the queue is greater than a sample size, then remove the first one
        if (previousVelocities.Count > 20)
        {
            previousVelocities.Dequeue();
        }
        
    }
}
