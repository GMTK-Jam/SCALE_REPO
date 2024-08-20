using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : Limb
{
    public Animator _anim;

/*    [Tooltip("anim speed multiplier, now deprecated")]
    public float[] animSpeeds;
    [Tooltip("anim speed multiplier, now deprecated")]
    public List<float> attackSpeeds;*/

    [Tooltip("Wait time in ANIMATION FRAMES. Reduces wait between arm swings as arm upgrades; by default there is a 15 frame hold (60 frame/s) when arm reaches top of swing; this is added on top")]
    public int[] animPauseFrames;

    public float[] armScales;
    

    public override int[] xpThresholds { get; } = { 100, 200, 300 }; // Example values
    public override int[] stagesWeight { get; } = { 100, 200, 300, 400 }; 


    void Start()
    {
        _anim = GetComponent<Animator>();
/*        _anim.speed = animSpeeds[0];
*/        transform.parent.localScale = new Vector3(armScales[0],armScales[0], armScales[0]);


    }

    public void HoldArmSwing()
    {
        StartCoroutine(HoldArmSwingRoutine());
    }

    private IEnumerator HoldArmSwingRoutine()
    {
        _anim.speed = 0;
        yield return new WaitForSeconds(animPauseFrames[stage]);
        _anim.speed = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
/*        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(5);
        }*/
    }

    public override void LevelUp()
    {
        if (stage < 4)
        {
            stage++;
        }
        transform.parent.localScale = new Vector3(armScales[stage], armScales[stage], armScales[stage]);

/*        _anim.speed = attackSpeeds[stage];
*/    }
}
