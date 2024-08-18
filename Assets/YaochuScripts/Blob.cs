using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Blob : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    /*DistanceJoint2D distJoint;
    Rigidbody2D rb;
    float scalar = 1.1f;*/

    private Vector3 targetPos;
    private bool dormant = true;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        /*rb = GetComponent<Rigidbody2D>();
        distJoint = GetComponent<DistanceJoint2D>();*/

        targetPos = transform.position;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grow()
    {

    }

    public void UpdateSize()
    {

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.alignment = TextAnchor.MiddleCenter;

        Handles.Label(transform.position, string.Concat(gameObject.name.Where(Char.IsNumber)),style);

/*        Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
*/    }
#endif

    /*    void UpdateMaxDist()
        {
            Bounds bounds = circleCollider.bounds;
            Vector3 size = bounds.size;
            distJoint.distance = size.x * scalar;
        }*/
}
