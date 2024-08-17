using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Eye : Limb
{

    public Camera _mainCamera;
    public CinemachineVirtualCamera vCam;
    public Player _player;
    public Transform eyeball;
    public CircleCollider2D eyeballAnchor;
    public List<float> FOVStages;
    private int stage = 0;

    // Start is called before the first frame update
    void Start()
    {
        vCam.m_Lens.OrthographicSize = FOVStages[0];
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateEyeball();
    }

    void UpdateEyeball()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 dest = eyeballAnchor.transform.position - mousePos;
        dest.Normalize();

        eyeball.position = eyeballAnchor.transform.position + dest * eyeballAnchor.radius;
    }

    void EnlargeView()
    {
        float currFOV = FOVStages[stage - 1];
        float newFOV = FOVStages[stage];
        Debug.Log("newFOV: " + newFOV);
        StartCoroutine(ChangeFOV(vCam, currFOV, newFOV, 2));
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
    }
}
