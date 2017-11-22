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

        for (int i = 0; i < ItemManager.intPotions; i++)
        {
            goPotionsUI.transform.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
            goPotionsUI.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().fillAmount = 0.0f;
        }

        for (int i = ItemManager.intPotions; i < 5; i++)
        {
            goPotionsUI.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().fillAmount = 0.0f;
        }
    }
	
	void Update ()
    {
        fHealth = healthSystemPlayer.intHP;
        imgHealthBar.fillAmount = fHealth / fMaxHealth;
        if (ItemManager.intPotions < 5)
        {
            goPotionsUI.transform.GetChild(ItemManager.intPotions).transform.GetChild(0).GetComponent<Image>().fillAmount = Mathf.Min(1.0f, ItemManager.fPotionFragments);
        }
	}

    public static void subRedrawPotions()
    {
        goPotionsUI.transform.GetChild(ItemManager.intPotions + 1).transform.GetChild(0).GetComponent<Image>().fillAmount = 0.0f;
        goPotionsUI.transform.GetChild(ItemManager.intPotions).transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
        goPotionsUI.transform.GetChild(ItemManager.intPotions).transform.GetChild(0).GetComponent<Image>().fillAmount = Mathf.Min(1.0f, ItemManager.fPotionFragments);
    }
}
