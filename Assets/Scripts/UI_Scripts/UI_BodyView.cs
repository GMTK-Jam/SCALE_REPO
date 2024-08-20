using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_BodyView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private TextMeshProUGUI displayText;
    void Start(){
        displayText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        var child = transform.GetChild(0).gameObject;
        if (child != null)
        {
            displayWeight();
            child.SetActive(true);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var child = transform.GetChild(0).gameObject;
        if (child != null)
        {
            child.SetActive(false);
        }
    }
    
    void displayWeight(){
        UI_Weight weight = transform.parent.GetComponent<UI_Weight>();
        string name = transform.name;
        weight.GetWeight();
        string preText = name + " (WEIGHT: ";
        string postText = ")";
        if (name == "Heart"){
            displayText.text = preText + weight.curr_heart_weight + postText;
        }
        else if (name == "Eye"){
            displayText.text = preText + weight.curr_eye_weight + postText;
        }
        else if (name == "Leg"){
            displayText.text = preText + weight.curr_leg_weight + postText;
        }
        else if (name == "Arm"){
            displayText.text = preText + weight.curr_arm_weight + postText;
        }
        else if (name == "Mouth"){
            displayText.text = preText + weight.curr_mouth_weight + postText;
        }
        else if (name == "Lung"){
            displayText.text = preText + weight.curr_lung_weight + postText;
        }
    }
}
