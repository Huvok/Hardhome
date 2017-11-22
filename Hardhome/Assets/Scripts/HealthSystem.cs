using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    int intMaxHP;
    public int intHP;
    Animator animator;
    public float fPotionFragmentsForKill;

	void Start ()
    {
        intMaxHP = intHP;
        animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
		if (intHP <= 0)
        {
            if (gameObject.tag != "Player")
            {
                ItemManager.fPotionFragments += fPotionFragmentsForKill;
                UIManager.subRedrawPotionFragments();
            }

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

    public void subRecoverHP(int intPointsToRecover)
    {
        intHP = Mathf.Min(intMaxHP, intHP + intPointsToRecover);
    }
}
