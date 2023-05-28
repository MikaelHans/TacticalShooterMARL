using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy : MonoBehaviour
{
    public Waypoint[] waypoints;
    public List<Vector3> waypointStaticLocations = new List<Vector3>();
    public List<Quaternion> waypointStaticRotations = new List<Quaternion>();

    private void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            waypointStaticLocations.Add(waypoint.transform.position);
            waypointStaticRotations.Add(waypoint.transform.rotation);
        }
    }
}
