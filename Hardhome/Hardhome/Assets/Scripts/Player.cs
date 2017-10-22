using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6f;

    Animator anim;
    Rigidbody2D rb2d;
    Vector2 v2Mov;

	void Start ()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
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
	}

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + v2Mov * speed * Time.deltaTime);
    }
}
