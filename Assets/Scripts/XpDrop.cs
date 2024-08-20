using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class XpDrop : MonoBehaviour
{
    private int _xp = 20;
    private bool _moving = false;
    public string xpType;

    public void SetXp(int xp)
    {
        _xp = xp;
    }

    void Update()
    {
        var player = Player.Instance;
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (!_moving && distanceToPlayer < player.pickupDistance)
        {
            _moving = true;
        }
        else if (_moving && distanceToPlayer >= player.pickupDistance)
        {
            _moving = false; // Stop chasing if the player moves out of range
        }

        if (_moving)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                player.pickupSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name.Contains("Blob"))
        {
            Debug.Log("attemptaddxp:" + gameObject.name);
            
            Player.Instance.AddXP(xpType, _xp);

            Destroy(gameObject);
        }
    }
}
