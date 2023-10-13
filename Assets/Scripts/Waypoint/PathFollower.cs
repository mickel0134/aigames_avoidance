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
        
    }

    /// <summary>
    /// This is the function that will check whether the agent has reached the target waypoint
    /// </summary>
    private bool TargetReached()
    {
        return false;
    }

    //This function returns true when a next target has been assigned, and false if there is no available next target
    private bool SetNextTarget()
    {
        return false;
    }

    public Vector3 Steer(Vector3 target)
    {
        return Vector3.zero;
    }
}
