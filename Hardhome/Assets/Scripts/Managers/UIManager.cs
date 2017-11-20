using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Image imgHealthBar;
    float fMaxHealth = 100f;
    public static float fHealth;

	void Start ()
    {
        imgHealthBar = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<Image>();
        fHealth = fMaxHealth;
	}
	
	void Update ()
    {
        imgHealthBar.fillAmount = fHealth / fMaxHealth;
	}
}
