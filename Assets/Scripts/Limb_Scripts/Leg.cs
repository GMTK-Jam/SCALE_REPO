using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.InputSystem;

public class Leg : Limb
{
    private Animator _anim;
    public Player _player;
    public Camera _mainCam;
    public Transform scaleAnchor;
    public float moveSpeed;
    [Range(0.1f,3)]public float animSpeed;


    public Animator _growthAnim;
    public float timeBetweenKeyframes = 1;

    public float mouseDistThresh;
    private Vector3 defaultScale;

    private bool isPushing = false;
    private float currAngle;

    private Vector3 mousePos;
    public override int[] xpThresholds { get; } = { 100, 200, 300 }; // Example values

    public List<float> animSpeedForLevels;
    public List<float> moveSpeedForLevels;
    private bool facingRight = true; // Tracks the current facing direction

    /*    public List<float> sizeForLevels;
        public List<float> xPosForLevels;*/

    [Range(0.1f, 5f)] public float growTime;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.speed = 0;
        _growthAnim.speed = 0;
        defaultScale = scaleAnchor.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        OnMouseMove();
    }

    public void OnMouseMove()
    {
        Vector2 dest = (Vector2)mousePos - (Vector2)_player.transform.position;
        if (dest.magnitude < mouseDistThresh)
        {
            dest = Vector2.zero;
        }
        dest.Normalize();

        Vector2 moveInput = dest;
        Vector2 velocity = moveInput * moveSpeed;

        if (velocity.magnitude > 0)
        {
            currAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            bool shouldFaceRight = moveInput.x > 0;

            if (shouldFaceRight != facingRight)
            {
                // Flip the leg
                facingRight = shouldFaceRight;
                scaleAnchor.localScale = new Vector3(scaleAnchor.localScale.x,
                                                     facingRight ? Mathf.Abs(scaleAnchor.localScale.y) : -Mathf.Abs(scaleAnchor.localScale.y),
                                                     defaultScale.z);
            }
        }

        if (dest.magnitude > 0)
        {
            _anim.speed = animSpeed;
            _player.Move(velocity, currAngle);
        }
        else
        {
            _anim.speed = 0;
            _player.Move(Vector2.zero, currAngle);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();
        Vector2 keyboardInput = Vector2.zero;

        keyboardInput.x = v.x;
        keyboardInput.y = v.y;
        keyboardInput.Normalize();

        Vector2 moveInput = keyboardInput;

        Vector2 velocity = moveInput * moveSpeed;

        if (velocity.magnitude > 0)
        {
            currAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            Debug.Log(currAngle);
        }

        if (context.started)
        {
            _anim.speed = 1;
            _player.Move(velocity, currAngle);
            
        }
        else if (context.canceled)
        {
            _anim.speed = 0;
            _player.Move(Vector2.zero, currAngle);
        }

    }

    public void LegIsPushing()
    {
        isPushing = true;
    }

    public void LegIsExtending()
    {
        isPushing = false;
    }


    private IEnumerator Extend()
    {
        _growthAnim.speed = 1;
        yield return new WaitForSeconds(timeBetweenKeyframes-0.01f);
        _growthAnim.speed = 0;

        /*float duration = growTime;
        float elapsed = 0;

        float currScale = sizeForLevels[stage-1];
        float targetScale = sizeForLevels[stage];

        float currXPos = scaleAnchor.localPosition.x;
        float targetXPos = scaleAnchor.localPosition.x + xPosForLevels[stage];

        while (elapsed < duration)
        {
            float s = Mathf.SmoothStep(currScale, targetScale, elapsed / duration);
            scaleAnchor.localScale = new Vector3(s, s, scaleAnchor.localScale.z);

            float xPos = Mathf.SmoothStep(currXPos, targetXPos, elapsed / duration);
            scaleAnchor.localPosition = new Vector3(xPos, scaleAnchor.localPosition.y, scaleAnchor.localPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }*/
    }

    private void SpeedUp()
    {
        moveSpeed = moveSpeedForLevels[stage];
        animSpeed = animSpeedForLevels[stage];
    }

    public override void LevelUp()
    {
        if (stage < 4)
        {
            stage++;
        }
        else
        {
            return;
        }
        StartCoroutine(Extend());
        SpeedUp();
    }
}
