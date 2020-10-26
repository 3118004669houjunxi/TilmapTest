using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    public Animator ani;
    public AnimatorStateInfo aniInfo;
    PlayerControl pc;
    public int status = 0;
    public bool canNext;
    void Start()
    {
        ani = GetComponent<Animator>();
        pc = transform.parent.GetComponent<PlayerControl>();
    }
    void Update()
    {
        aniInfo = ani.GetCurrentAnimatorStateInfo(0);
        if (((aniInfo.IsName("Attack") & pc.attackNum == 1) | (aniInfo.IsName("Attack 1") & pc.attackNum == 2) | (aniInfo.IsName("Attack 2") & pc.attackNum == 3)) & aniInfo.normalizedTime >= 1f)
        {
            if (canNext)
            {
                pc.attackNum++;
                pc.MP -= 2;
                canNext = false;
            }
            else
            {
                pc.attackNum = 0;
                pc.isAttack = false;
            }
        }
        else if ((aniInfo.IsName("Attack 3") & pc.attackNum == 4) & aniInfo.normalizedTime >= 1f)
        {
            if (canNext)
            {
                pc.attackNum = 1 ;
                pc.MP -= 2;
                canNext = false;
            }
            else
            {
                canNext = false;
                pc.attackNum = 0;
                pc.isAttack = false;
            }
        }
        if ((aniInfo.IsName("Attack") | aniInfo.IsName("Attack 1") | aniInfo.IsName("Attack 2") | aniInfo.IsName("Attack 3")) & aniInfo.normalizedTime > 0.5f & aniInfo.normalizedTime < 1f)
            if (Input.GetKeyDown(KeyCode.Z) & pc.canDo &pc.MP >= 2)
                canNext = true;
            /***/
        ani.SetInteger("status", status);
        if (pc.isOnGround & !pc.isMoveHold & !pc.isGroundDash & !pc.isAttack & !pc.isHitKeep)
            status = 0;
        if (pc.isMoveHold & pc.isOnGround)
            status = 1;
        if (pc.isJumpKeep)
            status = 2;
        if (!pc.isJumpKeep & !pc.isOnGround)
            status = 3;
        if (pc.isGroundDash)
            status = 4;
        if (pc.isAirDash)
            status = 5;
        if (pc.isAttack & pc.attackNum == 1)
            status = 6;
        if (pc.isAttack & pc.attackNum == 2)
            status = 7;
        if (pc.isAttack & pc.attackNum == 3)
            status = 8;
        if (pc.isAttack & pc.attackNum == 4)
            status = 9;
        if (pc.isHitKeep)
            status = 10;
        /**/
    }
    public void DoAttack()
    {
        pc.Attack();
    }
}
