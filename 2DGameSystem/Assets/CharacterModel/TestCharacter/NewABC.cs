using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewABC : MonoBehaviour
{
    BoxCollider2D bc;
    Rigidbody2D rb;
    public bool isJump;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isJump = Input.GetButtonDown("JumpUp");
    }
    void FixedUpdate()
    {
        Jump();
    }
    void Jump()
    {
        if (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 12f);
        }
    }
}
