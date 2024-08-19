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

    [Tooltip("The point from which the blob should grow out of, usually in the same hierarchy level as the blobs, defining a localPosition")]
    public Transform spawnPoint;
    private Vector3 targetPos;

    [Tooltip("Set to true if it should not be there when game starts")]
    public bool dormant = true;

    [Tooltip("How long to wait after growing one blob before growing the next blob in the level up animation, which grows a lot of cells sequentially")]
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

        Vector3 localSpawnPoint = spawnPoint.localPosition;

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
