using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    public float speed = 6f;

    Animator anim;
    Rigidbody2D rb2d;
    Vector2 v2Mov;

    public GameObject goInitialMap;

    private void Awake()
    {
        Assert.IsNotNull(goInitialMap);
    }

    void Start ()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        Camera.main.GetComponent<MainCamera>().subSetBounds(goInitialMap);
	}

	void Update ()
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
	}

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + v2Mov * speed * Time.deltaTime);
    }
}
