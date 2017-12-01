using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    int intMaxHP;
    public int intHP;
    Animator animator;
    public float fPotionFragmentsForKill;
    public ParticleSystem particleSystemDestroy;
    bool boolDestroying;
    public AudioClip[] arrAudioClipDamage;
    public AudioSource audioSourceDamage;

	void Start ()
    {
        intMaxHP = intHP;
        animator = GetComponent<Animator>();
        boolDestroying = false;
	}
	
	void Update ()
    {
		if (intHP <= 0 &&
            !boolDestroying)
        {
            boolDestroying = true;
            if (gameObject.tag != "Player")
            {
                if (ItemManager.intPotions < 5) ItemManager.fPotionFragments += fPotionFragmentsForKill;
                else ItemManager.fPotionFragments = 0.0f;
                UIManager.subRedrawPotionFragments();
            }

            if (animator != null &&
                animator.HasState(1, Animator.StringToHash("Destroy")))
            {
                MapManager.subOneMoreKill();
                StartCoroutine(subAnimateDestroying());
            }
            else
            {
                MapManager.subOneMoreKill();
                Destroy(gameObject);
            }
        }
	}

    private IEnumerator subAnimateDestroying()
    {
        animator.Play("Destroy");

        yield return new WaitForSeconds(1f);

        Destroy(Instantiate(particleSystemDestroy, transform.position, transform.rotation), 2f);

        if (gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        else if (gameObject.tag == "Alexa, the Pitiful Marauder")
        {
            GameObject.FindGameObjectWithTag("Cutscene Manager").GetComponent<CutsceneManager>().subTriggerEnding();
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void subReceiveDamage(int intDamage)
    {
        audioSourceDamage.clip = arrAudioClipDamage[Random.Range(0, arrAudioClipDamage.Length)];
        audioSourceDamage.Play();
        intHP -= intDamage;
    }

    public void subRecoverHP(int intPointsToRecover)
    {
        intHP = Mathf.Min(intMaxHP, intHP + intPointsToRecover);
    }
}
