using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEventListener : MonoBehaviour
{
    public CustomEvent Event;
    public UnityEvent Response;
    public bool oneShot = false;

    private void OnEnable()        
    { Event.RegisterListener(this); }        

    private void OnDisable()
    { Event.UnregisterListener(this); }

    private void OnDestroy()
    { Event.UnregisterListener(this); }

    public void OnEventRaised()
    {
        
        Response.Invoke();
        Debug.Log("Received event: "+Event.name);

        if (oneShot)
        {
            Event.UnregisterListener(this);
        }
    
    }
}        

