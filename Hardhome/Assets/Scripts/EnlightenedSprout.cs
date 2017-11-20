using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlightenedSprout : MonoBehaviour
{
    GameObject goPlayer;
    Rigidbody2D rb2d;
    Animator animator;
    HealthSystem healthSystem;
    InvincibilityTimer invincibilityTimer;

    private void Start()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        invincibilityTimer = GetComponent<InvincibilityTimer>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player_Sword_Zone" &&
            !invincibilityTimer.boolIsInvincible)
        {
            invincibilityTimer.boolIsInvincible = true;
            invincibilityTimer.fInvincibleTime = .3f;
            int intDamage = DamageManager.intGetDamage(col.tag);
            healthSystem.subReceiveDamage(intDamage);
            animator.Play("EP_Receive_Damage");
            int intForceReceived = DamageManager.intGetForce(col.tag);
            if (goPlayer.transform.GetChild(0).transform.localEulerAngles.z == 90)
            {
                rb2d.AddForce(new Vector2(intForceReceived, 0), ForceMode2D.Impulse);
            }
            else if (goPlayer.transform.GetChild(0).transform.localEulerAngles.z == 270)
            {
                rb2d.AddForce(new Vector2(-intForceReceived, 0), ForceMode2D.Impulse);
            }
            else if (goPlayer.transform.GetChild(0).transform.localEulerAngles.z == 180)
            {
                rb2d.AddForce(new Vector2(0, intForceReceived), ForceMode2D.Impulse);
            }
            else if (goPlayer.transform.GetChild(0).transform.localEulerAngles.z == 0)
            {
                rb2d.AddForce(new Vector2(0, -intForceReceived), ForceMode2D.Impulse);
            }
        }
    }
}
