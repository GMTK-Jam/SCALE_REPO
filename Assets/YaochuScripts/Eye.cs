using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Linq;
using System;

public class Eye : Limb
{

    public Camera _mainCamera;
    public CinemachineVirtualCamera vCam;
    public Player _player;
    public Transform eyeball;
    public CircleCollider2D eyeballAnchor;

    [Tooltip("Corresponding FOV for each level")]
    public List<float> FOVStages;

    [Tooltip("how many new blobs to generate for each level up")]
    public List<int> blobStages;

    private Queue<Blob> blobsToGenerate = new Queue<Blob>();
    public Transform eyeBlobParent;

    // Start is called before the first frame update
    void Start()
    {
        vCam.m_Lens.OrthographicSize = FOVStages[0];
        foreach (Transform child in eyeBlobParent)
        {
            Blob addBlob;
            if (child.gameObject.activeSelf == true)
            {
                continue;
            }
            if (child.TryGetComponent<Blob>(out addBlob))
            {
                blobsToGenerate.Enqueue(addBlob);
            }
        }
        blobsToGenerate = new Queue<Blob>(blobsToGenerate.OrderBy(b => Int32.Parse(string.Concat(b.gameObject.name.Where(Char.IsNumber)) )) );
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateEyeball();
    }

/*    void UpdateEyeball()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 dest = eyeballAnchor.transform.position - mousePos;
        dest.Normalize();

        eyeball.position = eyeballAnchor.transform.position + dest * eyeballAnchor.radius;
    }
*/
    void EnlargeView()
    {
        float currFOV = FOVStages[stage - 1];
        float newFOV = FOVStages[stage];
        Debug.Log("newFOV: " + newFOV);
        StartCoroutine(ChangeFOV(vCam, currFOV, newFOV, 2));
    }

    private IEnumerator BloatBody()
    {
        yield return null;
        int numToPop = blobStages[stage];
        while (numToPop > 0)
        {
            Blob newBlob = blobsToGenerate.Dequeue();
            StartCoroutine(newBlob.Grow());
            yield return new WaitForSeconds(newBlob.growWaitTime);
            newBlob.dormant = true;
            numToPop -= 1;
        }

    }

    IEnumerator ChangeFOV(CinemachineVirtualCamera cam, float startFOV, float endFOV, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            cam.m_Lens.OrthographicSize = Mathf.SmoothStep(startFOV, endFOV, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
    }

    public override void LevelUp()
    {
        Debug.Log("Eye Level Up");
        stage += 1;
        EnlargeView();
        StartCoroutine(BloatBody());
    }
}
