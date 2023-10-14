using UnityEngine;
using System.Collections;
using System;

public class ObstacleAvoidance : MonoBehaviour 
{
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField]
    private float rotationSpeed = 5.0f;
    [SerializeField]
    private float force = 50.0f;
    [SerializeField]
    private float minimumAvoidanceDistance = 10.0f;
    [SerializeField]
    private float avoidanceRadius = 3.0f;

    private float currentSpeed;
    private Vector3 targetPoint;
    private Vector3 direction;
    private Quaternion targetRotation;

    public LayerMask mask;
    // Use this for initialization
    void Start () 
    {
        targetPoint = Vector3.zero;
    }

 
	// Update is called once per frame
	private void Update () 
    {
        CheckInput();
        MoveAgent(); 
	}

    /// <summary>
    /// This is the function that will handle the movement of the vehicle
    /// </summary>
    private void MoveAgent()
    {
        //Calculate the directional vector towards the target point
        direction = targetPoint - transform.position;
        //Normalize the directional vector so that it's magniture is no more than 1.0
        direction.Normalize();

        //Modify the direction by applying obstacle avoidance
        ApplyAvoidance(ref direction);

        if (Vector3.Distance(targetPoint, transform.position) < avoidanceRadius)
            return;

        currentSpeed = movementSpeed * Time.deltaTime;

        targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * currentSpeed;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    /// <summary>
    /// This is the function that will handle the user's input
    /// </summary>
    private void CheckInput()
    {
        //Check whether the user has clicked the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            //Cast a ray from the main camera to the position of the mouse
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //If the ray hits something, assign the hit point as the target point
            if (Physics.Raycast(ray, out RaycastHit mouseHit, 100.0f))
            {
                targetPoint = mouseHit.point;
            }
        }
    }

    private void ApplyAvoidance(ref Vector3 direction)
    {
        //Add a layer mask that detect only a layer
        //0000 0000 0000 0000 0000 0001 0000 0000
        //This is a layerMask with 8 only set to true
        int layerMask = 1 << 8;
        
        //Check if the vehicle hits any obstacle within its minimum distance to avoid
        if(Physics.SphereCast(transform.position, avoidanceRadius, transform.forward, out RaycastHit hit, minimumAvoidanceDistance, layerMask))
        {
            //Get the normal of the hit point, since this is the vector perpendicular, the resultant of this..
            Vector3 hitNormal = hit.normal;

            //Don't want to move in the y-space
            hitNormal.y = 0;

            //...will point to the direction AWAY from the obstacle
            direction = transform.forward + hitNormal * force;
        }
    }
}