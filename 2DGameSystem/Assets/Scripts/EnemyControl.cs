using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("公开的属性")]
    public int HP = 100;
    public int ATK = 5;
    public float speed = 25;
    public float hitKeepTime = 0.2f;

    bool isInvincible = false;
    bool isFakeInvincible = false;
    bool isHitKeep;
    float hkT = 0;
    Vector2 hitEndSpeed = new Vector2();
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HitCheck();
    }
    public void GainPlayerDamage(int x)
    {
        if (!isInvincible)
        {
            HP -= x;
            hitEndSpeed.x = rb.velocity.x;
            hitEndSpeed.y = rb.velocity.y;
            hkT = Time.time;
            isHitKeep = true;
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
                hkT = 0;
            }
        }
    }
}
