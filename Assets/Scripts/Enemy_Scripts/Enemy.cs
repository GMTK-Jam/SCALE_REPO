using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _collisionDamage = 10;

    [SerializeField]
    public int hitpoints = 20;

    [SerializeField]
    private int _scoreValue = 10;

    [SerializeField]
    private int _xpValue = 10;

    [SerializeField]
    protected float _speed = 1f;

    [SerializeField]
    private DamageText _damageTextPrefab;

    [SerializeField]
    private XpDrop _xpDropPrefab;

    [SerializeField]
    private float _deathAnimDuration = 0.5f;

    [SerializeField]
    private float _damageCooldown = 2.0f; // Cooldown time in seconds

    [SerializeField]
    private float _separationDistance = 1.0f; // Minimum distance to maintain between enemies

    private SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidbody2D;
    private float _lastDamageTime; // Time since last damage taken

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _lastDamageTime = -_damageCooldown; // Initialize to allow immediate damage
    }

    // Update is called once per frame
    protected void Update()
    {
        // move towards the player character
        GameObject player = Player.Instance.gameObject;
        if (player != null)
        {
            var direction = DetermineDirection(player);
            Vector2 separation = AvoidOtherEnemies();
            _rigidbody2D.velocity = (direction + separation) * _speed;

            // flip sprite in x direction if moving left
            _spriteRenderer.flipX = direction.x < 0;
        }
    }

    virtual protected Vector2 DetermineDirection(GameObject player)
    {
        return Vector3.Normalize(player.transform.position - transform.position);
    }

    private Vector2 AvoidOtherEnemies()
    {
        Vector2 separation = Vector2.zero;
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (var enemy in enemies)
        {
            if (enemy != this)
            {
                Vector2 difference = transform.position - enemy.transform.position;
                float distance = difference.magnitude;

                if (distance < _separationDistance)
                {
                    separation += difference.normalized / distance; // Stronger separation effect when closer
                }
            }
        }

        return separation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - _lastDamageTime >= _damageCooldown)
        {
            Debug.Log(collision.gameObject.transform.name);
            if (collision.gameObject.GetComponent<Player>())
            {
                var player = collision.gameObject.GetComponent<Player>();
                DamagePlayer(player);

                TakeDamage(10);
            }
            _lastDamageTime = Time.time; // Update last damage time
        }
    }

    public void Knockback(Vector2 direction, float force)
    {
        _rigidbody2D.AddForce(direction * force);
    }

    public virtual void Death(bool leaveXp = false)
    {
        DisableHitboxes();

        transform
            .DOScale(0.01f, _deathAnimDuration)
            .OnComplete(() =>
            {
                if (leaveXp)
                {
                    var xpDrop = Instantiate(
                        _xpDropPrefab,
                        transform.position,
                        Quaternion.identity
                    );
                    xpDrop.SetXp(_xpValue);
                }
                Destroy(gameObject);
            });
    }

    protected void DisableHitboxes()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void DamagePlayer(Player player)
    {
        Debug.Log("Enemy.DamagePlayer: Player was damaged by " + gameObject.name);
        player.TakeDamage(_collisionDamage);
    }

    public void TakeDamage(int damage)
    {
        hitpoints -= damage;
        hitpoints = Mathf.Max(hitpoints, 0);

        var damageTextPosition = new Vector2(transform.position.x, transform.position.y + 1.0f);
        _damageTextPrefab.Spawn(transform.root.parent, damageTextPosition, damage);

        if (hitpoints == 0)
        {
            Death(leaveXp: true);
        }
    }
}
