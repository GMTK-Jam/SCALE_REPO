using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public Enemy_Ranged source { get; set; }
    public Transform target { get; set; }
    public ContactFilter2D filter { get; set; }
    public float speed { get; set; }
    public float range { get; set; }
    private Vector3 direction;
    private float distanceTravelled = 0;
    private bool exploding = false;

    private SpriteRenderer _renderer;
    public List<Sprite> sprites;


    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (target != null)
        {
            // Calculate the direction from the projectile to the target
            direction = (target.position - transform.position).normalized;
        }
        StartCoroutine(SwitchSprite(0));
    }



    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Move the projectile towards the target
            transform.position += direction * speed * Time.deltaTime;
            distanceTravelled += speed * Time.deltaTime;
            if (distanceTravelled > range)
            {
                speed = 0;
                Explode();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((filter.layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Blob damagedBlob = other.GetComponent<Blob>();
            source.ProjectileHit(damagedBlob.limbMaster);
            StartCoroutine(Explode());
        }
        else
        {

        }
    }

    private IEnumerator Explode()
    {
        exploding = true;
        _renderer.sprite = sprites[2];
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private IEnumerator SwitchSprite(int startIndex)
    {
        Vector3 defaultScale = transform.localScale;

        while (!exploding)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 1.2f));

            _renderer.sprite = sprites[startIndex + 1];
            transform.localScale = defaultScale;

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 1.2f));

            _renderer.sprite = sprites[startIndex];
            transform.localScale = new Vector3(defaultScale.x * 1.05f, defaultScale.y * 1.05f, defaultScale.z);
        }
    }
}
