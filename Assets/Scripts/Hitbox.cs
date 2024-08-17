using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Player _parentPlayer;

    public Player Player => _parentPlayer;

    void Awake()
    {
        _parentPlayer = transform.parent.GetComponent<Player>();
    }

    public void Disable()
    {
        // disable collider
        GetComponent<Collider2D>().enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _parentPlayer.Collide(collision.gameObject);
    }
}
