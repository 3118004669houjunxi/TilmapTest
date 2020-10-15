using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;
    [Header("状态")]
    public bool isMoveHold;
    public bool isJumpHold;
    public bool isOnGround;
    public bool isGroundDash;
    public bool isAirDash;
    public bool isHeadBlocked;
    [Header("参数")]
    float speed = 15f;
    float moveVar;
    float jumpForce = 25f;
    int jumpChance = 1;
    int jumpDownChance = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
        StateCheck();
    }
    void FixedUpdate()
    {
        
        Move();
        Jump();
        JumpDown();
    }
    void StateCheck()
    {
        if (rb.velocity.x != 0)
            isMoveHold = true;
        else
            isMoveHold = false;
        if (rb.velocity.y > 0)
            isJumpHold = true;
        else
            isJumpHold = false;
        if (Raycast(new Vector2(-0.5f, -0.9f), Vector2.down, 0.1f, LayerMask.GetMask("Ground"))|Raycast(new Vector2(0.5f, -0.9f), Vector2.down, 0.1f, LayerMask.GetMask("Ground")))
        {
            isOnGround = true;
            jumpChance = 1;
        }           
        else
            isOnGround = false;
        if (Raycast(new Vector2(-0.5f, 0.9f / (isGroundDash ? 2f : 1f)), Vector2.up, 0.1f, LayerMask.GetMask("Ground")) | Raycast(new Vector2(0.5f, 0.9f / (isGroundDash ? 2f : 1f)), Vector2.up, 0.1f, LayerMask.GetMask("Ground")))
            isHeadBlocked = true;
        else
            isHeadBlocked = false;
    }
    void Move()
    {
        moveVar = Input.GetAxis("Move");
        rb.velocity = new Vector2(moveVar * speed, rb.velocity.y);
    }
    void Jump()
    {
        if (Input.GetAxis("JumpUp") > 0 & isOnGround)
        {
            jumpDownChance = 1;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetButtonDown("JumpUp") & !isOnGround & jumpChance > 0)
        {
            jumpChance = 0;
            jumpDownChance = 1;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (!Input.GetButton("JumpUp") & rb.velocity.y < 24 & rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, 0);
    }
    void JumpDown()
    {
        if (Input.GetButtonDown("JumpDown") & !isOnGround & jumpDownChance > 0)
        {
            jumpDownChance = 0;
            rb.velocity = new Vector2(rb.velocity.x, -1.5f*jumpForce);
        }

    }
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float legth,LayerMask layer)
    {
        
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, legth, layer);
        Debug.DrawRay(pos + offset, rayDiraction, Color.red, legth);
        return hit;
    }
}
