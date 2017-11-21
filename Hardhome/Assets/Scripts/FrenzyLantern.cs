using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//                                                          //AUTHOR: Hugo Garcia 
//                                                          //Date: 11/20/2017 
//                                                          //PURPOSE: Frenzy Lantern monster script.

//====================================================================================================================== 
public class FrenzyLantern : MonoBehaviour
{
    public int intAngeryTrigger;
    public float fAngerySpeed;
    GameObject goPlayer;
    Rigidbody2D rb2d;
    Animator animator;
    HealthSystem healthSystem;
    InvincibilityTimer invincibilityTimer;
    Unit unit;

    //------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        invincibilityTimer = GetComponent<InvincibilityTimer>();
        unit = GetComponent<Unit>();
    }

    //------------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        if (healthSystem.intHP <= intAngeryTrigger &&
            !animator.GetBool("boolFLAngery"))
        {
            animator.SetBool("boolFLAngery", true);
            unit.speed = fAngerySpeed;
        }

        if (animator.GetBool("boolFLAngery") &&
            !animator.GetBool("boolFLAttacking") &&
            (transform.position - goPlayer.transform.position).sqrMagnitude < 7.5)
        {
            animator.SetBool("boolFLAttacking", true);
            StartCoroutine(subFLAttack());
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    IEnumerator subFLAttack()
    {
        float floatOriginalSpeed = unit.speed;
        unit.speed = 0;
        Vector2 v2DirToAttack = goPlayer.transform.position - transform.position;

        yield return new WaitForSeconds(.5f);

        float intTime = .3f;

        while (intTime > 0.0f)
        {
            intTime -= Time.deltaTime;
            rb2d.MovePosition(rb2d.position + v2DirToAttack * 4 * Time.deltaTime);

            yield return null;
        }

        unit.speed = floatOriginalSpeed;
        animator.SetBool("boolFLAttacking", false);
    }

    //------------------------------------------------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player_Sword_Zone" &&
            !invincibilityTimer.boolIsInvincible)
        {
            invincibilityTimer.boolIsInvincible = true;
            invincibilityTimer.fInvincibleTime = .3f;
            int intDamage = DamageManager.intGetDamage(col.tag);
            healthSystem.subReceiveDamage(intDamage);
            animator.Play("FL_Receive_Damage");
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

    //------------------------------------------------------------------------------------------------------------------
}
//====================================================================================================================== 