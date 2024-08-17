using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Leg : Limb
{
    private Animator _anim;
    public Player _player;
    public Camera _mainCam;
    public Transform flipAnchor;
    public float moveSpeed;
    [Range(0.1f,3)]public float animSpeed;

    public float mouseDistThresh;
    private Vector3 defaultScale;

    private bool isPushing = false;
    private float currAngle;

    private Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.speed = 0;
        defaultScale = flipAnchor.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        OnMouseMove();
    }


    public void OnMouseMove()
    {
        Vector2 dest = mousePos - _player.transform.position;
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
            if (moveInput.x < 0)
            {
                Debug.Log("flip");
                flipAnchor.localScale = new Vector3(defaultScale.x, -defaultScale.y, defaultScale.z);
            }
            else
            {
                flipAnchor.localScale = defaultScale;
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
}
