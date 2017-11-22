using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int intStartingPotions;
    public static int intPotions;
    public static float fPotionFragments;
    public int PotionStrength;
    public static int intPotionStrength;

    private void Awake()
    {
        intPotions = intStartingPotions;
        intPotionStrength = PotionStrength;
    }

    private void Update()
    {
        if (fPotionFragments >= 1.0f)
        {
            fPotionFragments -= 1.0f;
            intPotions++;
        }
    }
}
