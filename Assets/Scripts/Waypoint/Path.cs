using UnityEngine;
using System;

public class Path: MonoBehaviour
{
    [SerializeField]
    [Tooltip("The collection of waypoints")]
    private Transform[] waypoints;

    [Tooltip("Toggle to show the path of the waypoints")]
    public bool isDebug = true;
   
    [Tooltip("Defines the tolerance for pathfinding.")]
    public float radius = 2.0f;

    /// <summary>
    /// Returns the length of the waypoints array.
    /// </summary>
    public float PathLength {
        get { return waypoints.Length; }
    }

    /// <summary>
    /// Get a waypoint from the array in the given index.
    /// </summary>
    public Vector3 GetPoint(int index){
        return waypoints[index].position;
    }

    private void OnDrawGizmos(){
        if (!isDebug) {
            return;
        }
        for (int i = 0; i < waypoints.Length; i++){
            if (i + 1 < waypoints.Length){
                Debug.DrawLine(waypoints[i].position, waypoints[i + 1].position, Color.red);
            }
        }
    }
}