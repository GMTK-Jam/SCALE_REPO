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

    public Transform spawnPoint;
    private Vector3 targetPos;
    public bool dormant = true;
    [Range(0.1f, 5f)] public float growWaitTime;
    [Range(0.1f, 5f)] public float growTime;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        /*rb = GetComponent<Rigidbody2D>();
        distJoint = GetComponent<DistanceJoint2D>();*/

        targetPos = transform.localPosition;

        if (dormant)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Grow()
    {
        gameObject.SetActive(true);
        float duration = growTime;
        float elapsed = 0;

        Vector3 localSpawnPoint = spawnPoint.InverseTransformPoint(spawnPoint.position);

        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(localSpawnPoint, targetPos, Mathf.SmoothStep(0,1,elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
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
