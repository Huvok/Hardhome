using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public DamageLink[] arrDamages;
    static Dictionary<string, int> dictDamages;
    static Dictionary<string, int> dictForces;

    private void Start()
    {
        dictDamages = new Dictionary<string, int>();
        dictForces = new Dictionary<string, int>();
        for (int i = 0; i < arrDamages.Length; i++)
        {
            dictDamages.Add(arrDamages[i].strName, arrDamages[i].intDamage);
            dictForces.Add(arrDamages[i].strName, arrDamages[i].intForce);
        }
    }

    public static int intGetDamage(string str)
    {
        return dictDamages[str];
    }

    public static int intGetForce(string str)
    {
        return dictForces[str];
    }
}

[System.Serializable]
public struct DamageLink
{
    public string strName;
    public int intDamage;
    public int intForce;
}
