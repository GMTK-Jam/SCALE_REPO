using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public List<Limb> limbs;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

/*        foreach (Transform child in transform)
        {
            Limb limb;
            if (TryGetComponent<Limb>(out limb))
            {
                limbs.Add(limb);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb.velocity = moveInput* moveSpeed;
    }
}
