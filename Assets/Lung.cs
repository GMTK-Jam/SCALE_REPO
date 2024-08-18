using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Lung : Limb
{
    [Tooltip("How many new blobs to generate for each level up")]
    public List<int> blobStages;
    public List<float> flapSpeedStages;

    private Queue<Blob> blobsToGenerate = new Queue<Blob>();
    public Transform lungBlobParent;
    public Animator _lungSpriteAnim;
    public float timeBetweenKeyframes;
    public Arm arm;

    // Define the XP thresholds specific to Heart
    public override int[] xpThresholds { get; } = { 100, 200, 300 }; // Example values

    void Start()
    {
        // Initialize stage and XP
        stage = 0;
        xps = 0;

        // Set the animation speed to 0 at start
        _lungSpriteAnim.speed = 0;

        // Enqueue all inactive blobs in heartBlobParent
        foreach (Transform child in lungBlobParent)
        {
            if (child.TryGetComponent<Blob>(out Blob addBlob))
            {

                Debug.Log("Blob enqueued: " + addBlob.gameObject.name);
                blobsToGenerate.Enqueue(addBlob);

            }
        }

        // Order the blobs in the queue based on the numeric value in their name
        blobsToGenerate = new Queue<Blob>(blobsToGenerate.OrderBy(b => Int32.Parse(string.Concat(b.gameObject.name.Where(Char.IsNumber)))));
        Debug.Log("Total Blobs in Queue: " + blobsToGenerate.Count);
    }

    // Coroutine to "bloat" the body by growing the blobs
    private IEnumerator BloatBody()
    {
        int numToPop = blobStages[stage];
        while (numToPop > 0 && blobsToGenerate.Count > 0)
        {
            Blob newBlob = blobsToGenerate.Dequeue();
            StartCoroutine(newBlob.Grow());
            yield return new WaitForSeconds(newBlob.growWaitTime);
            newBlob.dormant = true;
            numToPop -= 1;
        }
    }

    // Coroutine to animate the heart bloating
    private IEnumerator BloatLung()
    {
        _lungSpriteAnim.speed = 1;
        yield return new WaitForSeconds(timeBetweenKeyframes);
        _lungSpriteAnim.speed = 0;
    }

    // LevelUp method overriding the abstract method in Limb
    public override void LevelUp()
    {
        StartCoroutine(BloatBody());
        StartCoroutine(BloatLung());

        // Move to the next stage after level-up
        if (stage < 4)
        {
            stage++;
            
        }
        arm._anim.speed = flapSpeedStages[stage];


    }
}
