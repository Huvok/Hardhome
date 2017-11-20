using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityTimer : MonoBehaviour
{
    public float fInvincibleTime;
    public bool boolIsInvincible;

    private void Start()
    {
        fInvincibleTime = 0f;
        boolIsInvincible = false;
    }

    void Update ()
    {
		if (fInvincibleTime > 0f)
        {
            boolIsInvincible = true;
            fInvincibleTime -= Time.deltaTime;
        }
        else
        {
            boolIsInvincible = false;
        }
	}
}
