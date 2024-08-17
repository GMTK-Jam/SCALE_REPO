using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class XpDrop : MonoBehaviour
{
    private int _xp = 10;
    private bool _moving = false;

    [SerializeField]
    private float _detectPlayerDistance = 3f;

    [SerializeField]
    private float _moveSpeed = 5f;

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
        if (
            !_moving
            && Vector2.Distance(player.transform.position, transform.position)
                < _detectPlayerDistance
        )
        {
            _moving = true;
        }

        if (_moving)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                _moveSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerHitbox")
        {
            var player = other.transform.parent.gameObject.GetComponent<Player>();
            player.AddScale(_xp);

            Destroy(gameObject);
        }
    }
}
