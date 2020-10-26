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
    public bool canDo = true;
    public bool canAttack = true;
    public bool isAttack = false;
    public bool isHitKeep;
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
    public bool isInvincible = false;
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
    float dashTime = 0.25f;   
    int wallJumpChance = 2;
    int maxAttackNum = 4;//最大连击编号
    public int attackNum = 0;//当前连击编号
    [Header("记录值")]
    Vector2 hitEndSpeed = new Vector2();//用于记录硬直前的速度
    float MPRecoverBeginTime;
    float MPRecoverTime = 2;
    float dashRealTime;//用于记录冲刺结束的游戏时间
    float invT = 0;//用于记录无敌触发后的游戏时间
    float hkT = 0;//用于记录硬直触发后的游戏时间
    float atkT = 0;//用于记录连击编号重置的游戏时间
    [Header("公有参数")]
    public float maxHP = 100;
    public float maxMP = 8;
    public float HP = 100;
    public float MP = 8;
    public int ATK = 10;
    public float invincibleTime = 2;
    
    public float hitKeepTime = 0.2f;
    public int gameProgress = 0;
    //Level,Collection and Buff...

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        size = bc.size;
        offset = bc.offset;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + new Vector3(isRight ? 0.5f : -0.5f, 0,0),1f);
    }
    void Update()
    {
        MPRecover();
        StartAttackTime();
        HitCheck();
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
        if (!isGroundDash & !isAirDash & !isHitKeep & Input.GetAxis("Move") != 0 & canDo & !isAttack)
            isMoveHold = true;
        else
            isMoveHold = false;
        if (rb.velocity.y > 0 & !isOnGround)
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
        {
            isRight = true;
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (Input.GetAxis("Move") < 0 & !isGroundDash & !isAirDash)
        {
            isRight = false;
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
    void Move()
    {
        if (isMoveHold)
        {
            moveVar = Input.GetAxis("Move");
            rb.velocity = new Vector2(moveVar * speed, rb.velocity.y);
        }
        else if(!isAirDash & !isGroundDash)
            rb.velocity = new Vector2(0, rb.velocity.y);

    }
    void Jump()
    {
        //基础跳
        if (jumpUpDown & isOnGround & !isGroundDash & !isHitKeep & canDo & !isAttack)
        {
            jumpDownChance = 1;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        //二段跳
        if (jumpUpDown & !isOnGround & jumpChance & !isAirDash & rb.velocity.y < 20 & ((!isLeftWall & !isRightWall) | wallJumpChance == 0) & !isHitKeep & canDo)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpChance = false;
            jumpDownChance = 1;
        }
        //掉落判定
        if (!jumpUpHold & rb.velocity.y < 24 & rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, 0);
        //墙壁跳
        if (jumpUpDown & !isOnGround & isLeftWall & wallJumpChance > 0 & !isAirDash & rb.velocity.y < 20 & !isHitKeep & canDo & !isAttack)
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
        if (jumpUpDown & !isOnGround & isRightWall & wallJumpChance > 0 & !isAirDash & rb.velocity.y < 20 & !isHitKeep & canDo & !isAttack)
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
        if (jumpDownDown & !isOnGround & jumpDownChance > 0 & !isAirDash & !isHitKeep & canDo)
        {
            jumpDownChance = 0;
            rb.velocity = new Vector2(rb.velocity.x, -1.5f * jumpForce);
        }
        if (jumpDownDown & isOnGround & !isGroundDash & !isHitKeep & canDo & !isAttack)
        {
            dashRealTime = Time.time + dashTime;
            bc.size = new Vector2(size.x, size.y / 2f);
            bc.offset = new Vector2(offset.x, offset.y - size.y / 4f);
            isGroundDash = true;
            CancelCanDo();
        }
        if (isGroundDash)
        {
            if (Time.time < dashRealTime & isOnGround)
                rb.velocity = new Vector2(2f * speed * (isRight ? 1f : -1f), rb.velocity.y);
            else if ((Time.time >= dashRealTime & !isHeadBlocked) | (Time.time < dashRealTime & !isOnGround))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                bc.offset = new Vector2(bc.offset.x, offset.y);
                bc.size = new Vector2(bc.size.x, size.y);
                isGroundDash = false;
                SetCanDo();
            }
        }
    }
    void AirDash()
    {
        if (airDashDown & !isOnGround & !isAirDash & !isHitKeep & canDo & !isAttack)
        {
            dashRealTime = Time.time + dashTime;
            isAirDash = true;
            CancelCanDo();
        }
        if (isAirDash)
        {
            if (Time.time < dashRealTime & !isRightWall & !isLeftWall)
                rb.velocity = new Vector2(2f * speed * (isRight ? 1f : -1f), 0);
            else
            {
                rb.velocity = new Vector2(0, 0);
                isAirDash = false;
                SetCanDo();
            }
        }
    }
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float legth, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, legth, layer);
        return hit;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        ContinuousDamage cd;
        if (collision.gameObject.TryGetComponent<ContinuousDamage>(out cd))
        {
            GainDamage(cd.Damage);
        }  
    }
    void UIUpdate()
    {
        MyHPBar.maxValue = maxHP;
        MyMPBar.maxValue = maxMP;
        MyHPBar.value = HP;
        MyMPBar.value = MP;
    }
    void StartAttackTime()
    {
        if (Input.GetKeyDown(KeyCode.Z) & !isAttack & canDo & isOnGround & canAttack & MP >= 2)
        {
            attackNum += 1;
            MP -= 2;
            isAttack = true;
        }
    }
    public void Attack()
    {  
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position + new Vector3(isRight?0.5f:-0.5f, 0, 0), 1f, 1 << LayerMask.NameToLayer("Enemies"));
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.GetComponent<EnemyControl>().GainPlayerDamage(ATK);
        }
    }
    public void GainDamage(int x)
    {
        if (!isInvincible)
        {
            HP -= x;
            if (!isAirDash & !isGroundDash & !isAttack)
            {
                hitEndSpeed.x = rb.velocity.x;
                hitEndSpeed.y = rb.velocity.y;
                hkT = Time.time;
                isHitKeep = true;
                CancelCanDo();
            }
            invT = Time.time;
            isInvincible = true;
        }      
    }
    void HitCheck()
    {
        if (isHitKeep)
        {
            rb.velocity = new Vector2(0, 0);
            if (hkT + hitKeepTime <= Time.time)
            {
                rb.velocity = hitEndSpeed;
                isHitKeep = false;
                SetCanDo();
                hkT = 0;
            }
        }
        if (isInvincible)
        {
            if (Time.time >= invT + invincibleTime)
            {
                isInvincible = false;
                invT = 0;
            }
        }
    }
    void MPRecover()
    {
        if (isAttack)
        {
            MPRecoverBeginTime = Time.time + MPRecoverTime;
        }
        if (Time.time >= MPRecoverBeginTime & MP < maxMP)
        {
            MP += 1/maxMP;
        }
    }
    public void CancelCanDo()
    {
        canDo = false;
    }
    public void SetCanDo()
    {
        canDo = true;
    }
}
