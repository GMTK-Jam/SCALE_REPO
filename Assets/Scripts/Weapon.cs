using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject pivotObject;
    public float rotationSpeed = 100f; // Rotation speed in degrees per second

    private void Update()
    {
        if (pivotObject != null)
        {
            transform.RotateAround(pivotObject.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(5);
        }
    }
}
