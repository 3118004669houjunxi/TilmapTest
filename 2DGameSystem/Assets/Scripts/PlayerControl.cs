using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;
    Vector2 size;
    Vector2 offset;
    [Header("UI")]
    public Slider MyHPBar;
    public Slider MyMPBar;
    [Header("人物状态")]
    public bool isMoveHold;
    public bool isRight;
    public bool isJumpKeep;
    public bool isOnGround;
    public bool isGroundDash;
    public bool isAirDash;
    public bool isHeadBlocked;
    public bool isRightWall;
    public bool isLeftWall;
    public bool canRightWallJump;
    public bool jumpChance;
    [Header("按键状态")]
    public bool jumpUpDown;
    public bool jumpUpHold;
    public bool jumpUpUp;
    public bool jumpDownDown;
    public bool airDashDown;
    [Header("私有参数")]
    float moveVar;
    float speed = 15f;
    float jumpForce = 25f;
    int jumpDownChance = 1;
    float dashTime = 0.5f;
    float dashRealTime;
    int wallJumpChance = 2;
    [Header("公有参数")]
    public int maxHP = 100;
    public int maxMP = 8;
    public int HP = 100;
    public int MP = 8;
    public int ATK = 10;
    public int gameProgress = 0;
    //Level,Collection and Buff...

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        size = bc.size;
        offset = bc.offset;
    }
    void Update()
    {
        ButtomCheck();
        StateCheck();
        Move();
        Jump();
        JumpDown();
        AirDash();
    }
    void FixedUpdate()
    {
        UIUpdate();
    }
    void ButtomCheck()
    {
        jumpUpDown = Input.GetButtonDown("JumpUp");
        jumpUpHold = Input.GetButton("JumpUp");
        jumpUpUp = Input.GetButtonDown("JumpUp");
        jumpDownDown = Input.GetButtonDown("JumpDown");
        airDashDown = Input.GetButtonDown("AirDash");
    }


    void StateCheck()
    {
        if (rb.velocity.x != 0 & !isGroundDash & !isAirDash)
            isMoveHold = true;
        else
            isMoveHold = false;
        if (rb.velocity.y > 0)
            isJumpKeep = true;
        else
            isJumpKeep = false;
        if (Raycast(new Vector2(-0.48f, -0.9f), Vector2.down, 0.1f, LayerMask.GetMask("Ground")) | Raycast(new Vector2(0.48f, -0.9f), Vector2.down, 0.1f, LayerMask.GetMask("Ground")))
        {
            isOnGround = true;
            jumpChance = true;
            wallJumpChance = 2;
        }
        else
            isOnGround = false;
        if (Raycast(new Vector2(0f, -0.9f), Vector2.up, 1f, LayerMask.GetMask("Ground")))
            isHeadBlocked = true;
        else
            isHeadBlocked = false;
        if (Raycast(new Vector2(-0.48f, -0.9f), Vector2.left, 0.1f, LayerMask.GetMask("Ground")))
            isLeftWall = true;
        else
            isLeftWall = false;
        if (Raycast(new Vector2(0.48f, -0.9f), Vector2.right, 0.1f, LayerMask.GetMask("Ground")))
            isRightWall = true;
        else
            isRightWall = false;
        if (Input.GetAxis("Move") > 0 & !isGroundDash & !isAirDash)
            isRight = true;
        else if (Input.GetAxis("Move") < 0 & !isGroundDash & !isAirDash)
            isRight = false;
    }
    void Move()
    {
        if (!isGroundDash & !isAirDash)
        {
            moveVar = Input.GetAxis("Move");
            rb.velocity = new Vector2(moveVar * speed, rb.velocity.y);
        }

    }
    void Jump()
    {
        if (jumpUpDown & isOnGround & !isGroundDash)
        {
            jumpDownChance = 1;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (jumpUpDown & !isOnGround & jumpChance & !isAirDash & rb.velocity.y < 20 & ((!isLeftWall & !isRightWall) | wallJumpChance == 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpChance = false;
            jumpDownChance = 1;

        }
        if (!jumpUpHold & rb.velocity.y < 24 & rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, 0);
        if (jumpUpDown & !isOnGround & isLeftWall & wallJumpChance > 0 & !isAirDash & rb.velocity.y < 20)
        {
            if (wallJumpChance == 2)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                wallJumpChance--;
                canRightWallJump = true;
            }
            else if (!canRightWallJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                wallJumpChance--;
                canRightWallJump = true;
            }
        }
        if (jumpUpDown & !isOnGround & isRightWall & wallJumpChance > 0 & !isAirDash & rb.velocity.y < 20)
        {
            if (wallJumpChance == 2)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                wallJumpChance--;
                canRightWallJump = false;
            }
            else if (canRightWallJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                wallJumpChance--;
                canRightWallJump = false;
            }
        }
    }
    void JumpDown()
    {
        if (jumpDownDown & !isOnGround & jumpDownChance > 0 & !isAirDash)
        {
            jumpDownChance = 0;
            rb.velocity = new Vector2(rb.velocity.x, -1.5f * jumpForce);
        }
        if (jumpDownDown & isOnGround & !isGroundDash)
        {

            dashRealTime = Time.time + dashTime;
            bc.size = new Vector2(size.x, size.y / 2f);
            bc.offset = new Vector2(offset.x, offset.y - size.y / 4f);
            isGroundDash = true;
        }
        if (isGroundDash)
        {
            if (Time.time < dashRealTime & isOnGround)
                rb.velocity = new Vector2(1.5f * speed * (isRight ? 1f : -1f), rb.velocity.y);
            else if ((Time.time >= dashRealTime & !isHeadBlocked) | (Time.time < dashRealTime & !isOnGround))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                bc.offset = new Vector2(bc.offset.x, offset.y);
                bc.size = new Vector2(bc.size.x, size.y);
                isGroundDash = false;
            }
        }
    }
    void AirDash()
    {
        if (airDashDown & !isOnGround & !isAirDash)
        {
            dashRealTime = Time.time + dashTime;
            isAirDash = true;
        }
        if (isAirDash)
        {
            if (Time.time < dashRealTime & !isRightWall & !isLeftWall)
                rb.velocity = new Vector2(1.5f * speed * (isRight ? 1f : -1f), 0);
            else
            {
                rb.velocity = new Vector2(0, 0);
                isAirDash = false;
            }
        }
    }
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float legth, LayerMask layer)
    {

        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, legth, layer);
        return hit;
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "")
        {
        }
        
    }*/
    void UIUpdate()
    {
        MyHPBar.maxValue = maxHP;
        MyMPBar.maxValue = maxMP;
        MyHPBar.value = HP;
        MyMPBar.value = MP;
    }
}
