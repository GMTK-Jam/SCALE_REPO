using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_BodyView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        var child = transform.GetChild(0).gameObject;
        if (child != null)
        {
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
    

}
