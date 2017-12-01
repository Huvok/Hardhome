using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Image imgHealthBar;
    float fMaxHealth = 100f;
    public static float fHealth;
    public GameObject goPotionsBar;
    public static GameObject goPotionsUI;
    HealthSystem healthSystemPlayer;

	void Start ()
    {
        healthSystemPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        imgHealthBar = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<Image>();
        fHealth = fMaxHealth;
        goPotionsUI = goPotionsBar;
        subRedrawPotions();
    }
	
	void Update ()
    {
        fHealth = healthSystemPlayer.intHP;
        imgHealthBar.fillAmount = fHealth / fMaxHealth;
	}

    public static void subRedrawPotions()
    {
        for (int i = 0; i < ItemManager.intPotions; i++)
        {
            goPotionsUI.transform.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
        }

        for (int i = ItemManager.intPotions; i < 5; i++)
        {
            goPotionsUI.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().fillAmount = 0.0f;
            goPotionsUI.transform.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
        }

        if (ItemManager.intPotions < goPotionsUI.transform.childCount)
            goPotionsUI.transform.GetChild(ItemManager.intPotions).transform.GetChild(0).GetComponent<Image>().fillAmount = Mathf.Min(1.0f, ItemManager.fPotionFragments);
    }

    public static void subRedrawPotionFragments()
    {
        if (ItemManager.intPotions < 5)
        {
            goPotionsUI.transform.GetChild(ItemManager.intPotions).transform.GetChild(0).GetComponent<Image>().fillAmount = Mathf.Min(1.0f, ItemManager.fPotionFragments);
        }
    }
}
