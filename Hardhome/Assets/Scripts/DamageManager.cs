using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public DamageLink[] arrDamages;
    static Dictionary<string, int> dictDamages;

    private void Start()
    {
        dictDamages = new Dictionary<string, int>();
        for (int i = 0; i < arrDamages.Length; i++)
        {
            dictDamages.Add(arrDamages[i].strName, arrDamages[i].intDamage);
        }
    }

    public static int intFind(string str)
    {
        return dictDamages[str];
    }
}

[System.Serializable]
public struct DamageLink
{
    public string strName;
    public int intDamage;
}
