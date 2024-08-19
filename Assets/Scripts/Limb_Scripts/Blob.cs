using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Blob : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;

    [Tooltip("The point from which the blob should grow out of, usually in the same hierarchy level as the blobs, defining a localPosition")]
    public Transform spawnPoint;
    private Vector3 targetPos;

    [Tooltip("Set to true if it should not be there when game starts")]
    public bool dormant = true;

    [Tooltip("How long to wait after growing one blob before growing the next blob in the level up animation, which grows a lot of cells sequentially")]
    [Range(0.1f, 5f)] public float growWaitTime;
    [Range(0.1f, 5f)] public float growTime;
    [SerializeField]
    private Sprite[] sprites;
    private int randomIndex;
    public bool changeSort = true;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        targetPos = transform.localPosition;

        if (dormant)
        {
            gameObject.SetActive(false);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        // Validate the sprites
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] == null)
            {
                Debug.LogError($"Sprite at index {i} failed to load!");
            }
            else
            {
                Debug.Log($"Sprite at index {i} loaded successfully: {sprites[i].name}");
            }
        }
    }

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Randomly select either A, B, or C when the blob is enabled
        randomIndex = UnityEngine.Random.Range(0, 3) * 2; // This will give 0, 2, or 4
        spriteRenderer.sprite = sprites[randomIndex]; // Set to -1 graphic

        StartCoroutine(SwitchSprite(randomIndex));

        if (changeSort)
        {
            int blobCount = CountObjectsWithLayer("Blob");
            spriteRenderer.sortingOrder = -110 - blobCount;
        }
    }

    int CountObjectsWithLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                count++;
            }
        }

        return count;
    }

    private IEnumerator SwitchSprite(int startIndex)
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 1.2f));

            spriteRenderer.sprite = sprites[startIndex + 1];

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 1.2f));

            spriteRenderer.sprite = sprites[startIndex];
        }
    }

    public IEnumerator Grow()
    {
        gameObject.SetActive(true);
        float duration = growTime;
        float elapsed = 0;

        Vector3 localSpawnPoint = spawnPoint.localPosition;

        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(localSpawnPoint, targetPos, Mathf.SmoothStep(0, 1, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void UpdateSize()
    {
        // Implement size update logic here
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.alignment = TextAnchor.MiddleCenter;

        Handles.Label(transform.position, string.Concat(gameObject.name.Where(Char.IsNumber)), style);
    }
#endif
}
