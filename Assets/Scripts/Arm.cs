using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private Animator _anim;

    [Range(0.1f, 3)] public float animSpeed;

    void Start()
    {
        _anim = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _anim.speed = animSpeed;

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
