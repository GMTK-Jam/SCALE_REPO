using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{

    DistanceJoint2D distJoint;
    Rigidbody2D rb;
    Collider2D circleCollider;
    float scalar = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        distJoint = GetComponent<DistanceJoint2D>();
        UpdateMaxDist();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateSize()
    {

    }

    void UpdateMaxDist()
    {
        Bounds bounds = circleCollider.bounds;
        Vector3 size = bounds.size;
        distJoint.distance = size.x * scalar;
    }
}
