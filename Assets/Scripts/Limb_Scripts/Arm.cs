using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : Limb
{
    public Animator _anim;

    public float[] animSpeeds;
    public float[] armScales;

    public override int[] xpThresholds { get; } = { 100, 200, 300 }; // Example values
    public override int[] stagesWeight { get; } = { 100, 200, 300, 400 }; 


    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.speed = animSpeeds[0];
        transform.parent.localScale = new Vector3(armScales[0],armScales[0], armScales[0]);


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
        if (stage < 4)
        {
            stage++;
        }
        transform.parent.parent.localScale = new Vector3(armScales[stage], armScales[stage], armScales[stage]);

        //_anim.speed = animSpeeds[stage];
    }
}
