using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomEvent")]
public class CustomEvent : ScriptableObject
{
    protected List<CustomEventListener> listeners = new List<CustomEventListener>();
    public bool debug = true;

    public void RegisterListener(CustomEventListener client)
    {
        listeners.Add(client);
    }

    public void UnregisterListener(CustomEventListener client)
    {
        listeners.Remove(client);
    }

    public void Raise()
    {
        if (debug)
        {
            Debug.Log("Event " + this.name + " raised");
        }
        foreach (CustomEventListener listener in listeners)
        {
            listener.OnEventRaised();
        }
    }
}

public class DamageInfo
{
    public float damage;
    public EnemyClass enemyClass;
    public LimbClass limbClass;

    public DamageInfo(float damage, EnemyClass enemyClass, LimbClass limbClass)
    {
        this.damage = damage;
        this.enemyClass = enemyClass;
        this.limbClass = limbClass;
    }
}