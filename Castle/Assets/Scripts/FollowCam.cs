using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;

    private void Awake()
    {
        camZ = transform.position.z;
    }
    private void FixedUpdate()
    {
        if(POI == null) { return; }
        Vector3 destination = POI.transform.position;
        destination.x = Mathf.Max(destination.x, minXY.x);
        destination.y = Mathf.Max(destination.y, minXY.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }
}
