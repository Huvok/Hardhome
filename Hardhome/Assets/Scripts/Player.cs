﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    public float speed = 6f;

    Animator anim;
    Rigidbody2D rb2d;
    Vector2 v2Mov;

    PolygonCollider2D attackCollider;
    DialogueManager dialogueManager;
    CutsceneManager cutsceneManager;

    public GameObject goInitialMap;
    HealthSystem healthSystem;

    private void Awake()
    {
        Assert.IsNotNull(goInitialMap);
    }

    void Start ()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        attackCollider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        attackCollider.enabled = false;

        Camera.main.GetComponent<MainCamera>().subSetBounds(goInitialMap);
        dialogueManager = GameObject.FindGameObjectWithTag("Dialogue Manager").GetComponent<DialogueManager>();
        cutsceneManager = GameObject.FindGameObjectWithTag("Cutscene Manager").GetComponent<CutsceneManager>();
        healthSystem = GetComponent<HealthSystem>();
	}

	void Update ()
    {
        if (!dialogueManager.boolOnDialogue)
        {
            v2Mov = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (v2Mov != Vector2.zero)
            {
                anim.SetFloat("movX", v2Mov.x);
                anim.SetFloat("movY", v2Mov.y);
                anim.SetBool("boolPlayerWalking", true);
            }
            else
            {
                anim.SetBool("boolPlayerWalking", false);
            }

            AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            bool boolAttacking = animStateInfo.IsName("Player Attack");

            if (Input.GetMouseButtonDown(0) && !boolAttacking)
            {
                anim.SetTrigger("Attack");
            }

            if (v2Mov != Vector2.zero)
            {
                if (v2Mov.x == -1)
                {
                    attackCollider.offset = new Vector2(0f, 0f);
                    transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, -90);
                }
                else if (v2Mov.x == 1)
                {
                    attackCollider.offset = new Vector2(0f, 0f);
                    transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 90);
                }
                else if (v2Mov.y == 1)
                {
                    attackCollider.offset = new Vector2(0f, -.3f);
                    transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 180);
                }
                else if (v2Mov.y == -1)
                {
                    attackCollider.offset = new Vector2(0f, -.3f);
                    transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }

            if (boolAttacking)
            {
                float numPlaybackTime = animStateInfo.normalizedTime;
                if (numPlaybackTime > .33f &&
                    numPlaybackTime < .66f)
                {
                    attackCollider.enabled = true;
                }
                else
                {
                    attackCollider.enabled = false;
                }
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.StartsWith("Damage"))
        {
            int intDamage = DamageManager.intFind(collision.name.Substring(collision.name.IndexOf("-") + 1));
            UIManager.fHealth -= intDamage;
            healthSystem.subReceiveDamage(intDamage);
            anim.Play("Receive_Damage");
        }
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + v2Mov * speed * Time.deltaTime);
    }

    public void subTriggerDialogue(string strDialogue)
    {
        cutsceneManager.subTriggerDialogue(strDialogue);
    }

    public void subToggleSpriteOn(string str)
    {
        cutsceneManager.subToggleSpriteOn(str);
    }

    public void subToggleSpriteOff(string str)
    {
        cutsceneManager.subToggleSpriteOff(str);
    }

    public void subTriggerNextOnQueue()
    {
        cutsceneManager.subTriggerNextOnQueue();
    }
}
