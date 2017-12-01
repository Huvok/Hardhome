using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public KillCountNeeded[] arrKillCountNeeded;
    public EnergyWall[] arrEnergyWalls;

    public static Dictionary<string, int> dictKillCountNeeded;
    public static Dictionary<string, List<GameObject>> dictEnergyWalls;
    public static string strCurrentMap;

    void Start ()
    {
        dictKillCountNeeded = new Dictionary<string, int>();
        dictEnergyWalls = new Dictionary<string, List<GameObject>>();
		for (int i = 0; i < arrKillCountNeeded.Length; i++)
        {
            dictKillCountNeeded.Add(arrKillCountNeeded[i].strMap, arrKillCountNeeded[i].intCount);
        }

        for (int i = 0; i < arrEnergyWalls.Length; i++)
        {
            dictEnergyWalls.Add(arrEnergyWalls[i].strMap, arrEnergyWalls[i].lstGoEnergyWall);
            
            for (int j = 0; j < arrEnergyWalls[i].lstGoEnergyWall.Count; j++)
            {
                arrEnergyWalls[i].lstGoEnergyWall[j].SetActive(false);
            }
        }
	}

    public static void subActivateWalls(string strMap)
    {
        for (int i = 0; i < dictEnergyWalls[strMap].Count; i++)
        {
            dictEnergyWalls[strMap][i].SetActive(true);
        }
    }

    public static void subDeactivateWalls(string strMap)
    {
        for (int i = 0; i < dictEnergyWalls[strMap].Count; i++)
        {
            dictEnergyWalls[strMap][i].SetActive(false);
        }
    }

    public static void subOneMoreKill()
    {
        dictKillCountNeeded[strCurrentMap]--;
        if (dictKillCountNeeded[strCurrentMap] <= 0)
        {
            if (strCurrentMap == "Ancient Grove" ||
                strCurrentMap == "Forsaken Field" ||
                strCurrentMap == "Swept Cathedral" ||
                strCurrentMap == "Silent Ghetto")
            {
                subDeactivateWalls(strCurrentMap);
            }

            if (strCurrentMap == "Forsaken Field")
            {
                GameManager.boolForsakenFieldCleared = true;
            }
            else if (strCurrentMap == "Swept Cathedral")
            {
                GameManager.boolSweptCathedralCleared = true;
            }
        }
    }
}

[System.Serializable]
public struct KillCountNeeded
{
    public string strMap;
    public int intCount;
}

[System.Serializable]
public struct EnergyWall
{
    public string strMap;
    public List<GameObject> lstGoEnergyWall;
}