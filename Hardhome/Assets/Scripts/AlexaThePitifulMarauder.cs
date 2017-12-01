using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//                                                          //AUTHOR: Hugo Garcia 
//                                                          //Date: 11/20/2017 
//                                                          //PURPOSE: Alexa, the Pitiful Marauder boss script.

//====================================================================================================================== 
public class AlexaThePitifulMarauder : MonoBehaviour
{
    public int intSecondPhaseTrigger;
    public float fSecondPhaseSpeed;
    GameObject goPlayer;
    Rigidbody2D rb2d;
    Animator animator;
    HealthSystem healthSystem;
    InvincibilityTimer invincibilityTimer;
    Unit unit;
    PolygonCollider2D attackCollider;

    //------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        invincibilityTimer = GetComponent<InvincibilityTimer>();
        unit = GetComponent<Unit>();
        attackCollider = transform.GetChild(2).GetComponent<PolygonCollider2D>();
        attackCollider.enabled = false;
    }

    //------------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        if (healthSystem.intHP <= intSecondPhaseTrigger &&
            !animator.GetBool("boolATPMAttacking"))
        {
            unit.speed = fSecondPhaseSpeed;
        }

        if (!animator.GetBool("boolATPMAttacking") &&
            (transform.position - goPlayer.transform.position).sqrMagnitude < 10)
        {
            animator.SetBool("boolATPMAttacking", true);
            StartCoroutine(subATPMAttack());
        }

        if (animator.GetFloat("movY") == -1.0f)
        {
            attackCollider.offset = new Vector2(0f, -.3f);
            transform.GetChild(2).transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (animator.GetFloat("movX") == -1.0f)
        {
            attackCollider.offset = new Vector2(0f, 0f);
            transform.GetChild(2).transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else if (animator.GetFloat("movX") == 1.0f)
        {
            attackCollider.offset = new Vector2(0f, 0f);
            transform.GetChild(2).transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (animator.GetFloat("movY") == 1.0f)
        {
            attackCollider.offset = new Vector2(0f, -.3f);
            transform.GetChild(2).transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    IEnumerator subATPMAttack()
    {
        float floatOriginalSpeed = unit.speed;
        unit.speed = 0;

        yield return new WaitForSeconds(1.0f);
        Vector2 v2DirToAttack = goPlayer.transform.position - transform.position;
        yield return new WaitForSeconds(.5f);
        animator.SetTrigger("triggerAttack");
        attackCollider.enabled = true;

        float intTime = .2f;

        while (intTime > 0.0f)
        {
            intTime -= Time.deltaTime;
            rb2d.MovePosition(rb2d.position + v2DirToAttack * 5 * Time.deltaTime);

            yield return null;
        }

        attackCollider.enabled = false;
        unit.speed = floatOriginalSpeed;
        animator.SetBool("boolATPMAttacking", false);
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
            animator.Play("ATPM_Receive_Damage");
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
//====================================================================================================================== 