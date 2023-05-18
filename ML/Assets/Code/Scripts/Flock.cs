using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
	internal FlockController controller;

    private new Rigidbody rigidbody;

    private Vector3 randomize;
    private Color debugColor = Color.green;

    // private Vector3 avoidDirection;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 direction = (controller.targetPoint - transform.position);
        direction.Normalize();

        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);

        Vector3 forward = transform.TransformDirection(Vector3.forward) * controller.minimumDistToAvoid;

        Debug.DrawRay(transform.position, forward, debugColor);
    }

    void FixedUpdate()
    {
        if (controller)
        {
            Vector3 relativePos = Steer() * Time.deltaTime;

            AvoidObstacles(ref relativePos);

            if(relativePos != Vector3.zero)
                rigidbody.velocity = relativePos;

            float speed = rigidbody.velocity.magnitude;

            if (speed > controller.maxVelocity)
                rigidbody.velocity = rigidbody.velocity.normalized * controller.maxVelocity;
                
            else if (speed < controller.minVelocity) 
                rigidbody.velocity = rigidbody.velocity.normalized * controller.minVelocity;
        }
    }

    public void AvoidObstacles(ref Vector3 dir) 
    {
        int layerMask = 1 << 8;
        
        debugColor = Color.green;

        if (Physics.SphereCast(transform.position, controller.boidRadius, transform.forward, out var hit, controller.minimumDistToAvoid, layerMask)) 
        {
            Vector3 hitNormal = hit.normal;
            Vector3 tangent = Vector3.Cross( hitNormal, Vector3.forward );

            if( tangent.magnitude == 0 )
            {
                tangent = Vector3.Cross( hitNormal, Vector3.up );
            }

            dir += tangent + hitNormal * controller.force;

            debugColor = Color.red;
        }

    }

    //Calculate flock steering Vector based on the Craig Reynold's algorithm (Cohesion, Alignment, Follow leader and Seperation)
    private Vector3 Steer() 
    {
        Vector3 center = controller.flockCenter - transform.localPosition;          // cohesion
        Vector3 velocity = controller.flockVelocity - rigidbody.velocity;           // alignment
        Vector3 follow = controller.targetPoint - transform.localPosition;          // follow leader
        Vector3 separation = Vector3.zero; 											// separation

        foreach (Flock flock in controller.flockList) 
        {
            if (flock != this) 
            {
                Vector3 relativePos = transform.localPosition - flock.transform.localPosition;
                separation += relativePos.normalized;
            }
        }

        // Randomize the direction every 2 seconds
        if(Time.time % 2 ==0)
        {
            randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);
            randomize.Normalize();
        }

        return (controller.centerWeight * center +
                controller.velocityWeight * velocity +
                controller.separationWeight * separation +
                controller.followWeight * follow +
                controller.randomizeWeight * randomize);
    }	
}
