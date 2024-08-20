using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Weight : MonoBehaviour
{
    private Player player;
    private Heart heart;
    private Eye eye;
    private Leg leg;
    private Arm arm;
    private Mouth mouth;
    private Lung lung;

    public float curr_heart_weight;
    public float curr_eye_weight;
    public float curr_leg_weight;
    public float curr_arm_weight;
    public float curr_mouth_weight;
    public float curr_lung_weight;

    void Start(){
        player = GameObject.Find("Human").GetComponent<Player>();
        heart = player.heart;
        eye = player.eye;
        leg = player.leg;
        arm = player.arm;
        mouth = player.mouth;
        lung = player.lung;
    }

    public void GetWeight(){
        curr_heart_weight = heart.stagesWeight[heart.stage];
        curr_eye_weight = eye.stagesWeight[eye.stage];
        curr_leg_weight = leg.stagesWeight[leg.stage];
        curr_arm_weight = arm.stagesWeight[arm.stage];
        curr_mouth_weight = mouth.stagesWeight[mouth.stage];
        curr_lung_weight = lung.stagesWeight[lung.stage];
    }


}
