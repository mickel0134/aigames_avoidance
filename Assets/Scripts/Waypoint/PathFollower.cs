using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField]
    private Path path;
    [SerializeField]
    private float speed = 20.0f;
    [Range(1.0f, 1000.0f)]
    public float steeringInertia = 100.0f;
    [SerializeField]
    private bool isLooping = true;

    private float currentSpeed = 0;
    private int currentPathIndex = 0;

    private Vector3 targetPoint;
    private Vector3 velocity;

    private void Start()
    {
        // We get the firt point along the path
        targetPoint = path.GetPoint(currentPathIndex);
    }

    // Update is called once per frame
    void Update()
    {
        MoveAgent();
    }

    /// <summary>
    /// This is the function that will handle the movement of the agent
    /// </summary>
    private void MoveAgent()
    {
        //Checker - always ensure that there is a path available
        //This is to avoid null reference errors
        if(path == null)
        {
            Debug.LogWarning("Please assign a path for the agent.");
            return;
        }

        //Update the speed of the agent
        currentSpeed = speed * Time.deltaTime;
        //Check whether target has been reached
        if (TargetReached())
        {
            //Determine the next waypoint. Dont do anything else when last point is reached
            if (!SetNextTarget())
            {
                return;
            }
        }

        velocity += Steer(targetPoint);
        //Apply this to the transform
        transform.position += velocity;
        //Make the agent look towards the direction
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    /// <summary>
    /// This is the function that will check whether the agent has reached the target waypoint
    /// </summary>
    private bool TargetReached()
    {
        return (Vector3.Distance(transform.position, targetPoint) < path.radius);
    }

    //This function returns true when a next target has been assigned, and false if there is no available next target
    private bool SetNextTarget()
    {
        bool success = false;
        //Increment the value of the currentPathIndex if the agent hasn't reached the end of the path's array
        if(currentPathIndex < path.PathLength - 1)
        {
            currentPathIndex++;
            success = true;
        }
        //Check whether isLooping is set to true to reset the index
        else if(isLooping)
        {
            currentPathIndex = 0;
            success = true;
        }
        //Make sure to assign the targetPoint
        targetPoint = path.GetPoint(currentPathIndex);
        return success;
    }

    public Vector3 Steer(Vector3 target)
    {
        //Get the directional vector from point a to b
        Vector3 desiredVelocity = (target - transform.position);
        //Normalize the desired velocity
        desiredVelocity.Normalize();
        //Apply the speed
        desiredVelocity *= currentSpeed;
        //Calculate the force vector
        Vector3 steeringForce = desiredVelocity - velocity;
        //Change the return value to the steering direction
        return steeringForce / steeringInertia;
    }
}
