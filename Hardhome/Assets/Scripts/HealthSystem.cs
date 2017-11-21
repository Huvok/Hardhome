﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int intHP;
    Animator animator;

	void Start ()
    {
        animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
		if (intHP <= 0)
        {
            if (animator != null &&
                animator.HasState(1, Animator.StringToHash("Destroy")))
            {
                StartCoroutine(subAnimateDestroying());
            }
            else
            {
                Destroy(gameObject);
            }
        }
	}

    private IEnumerator subAnimateDestroying()
    {
        animator.Play("Destroy");

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Destroy"))
        {
            yield return null;
        }

        if (gameObject.tag != "Player")
            Destroy(gameObject);
    }

    public void subReceiveDamage(int intDamage)
    {
        intHP -= intDamage;
    }
}
