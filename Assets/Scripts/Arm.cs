using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : Limb
{
    private Animator _anim;

    public float[] animSpeeds;
    public override int[] xpThresholds { get; } = { 100, 200, 300 }; // Example values

    void Start()
    {
        _anim = transform.parent.GetComponent<Animator>();
        _anim.speed = animSpeeds[0];

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(5);
        }
    }

    public override void LevelUp()
    {
        stage += 1;

        _anim.speed = animSpeeds[stage];
    }
}
