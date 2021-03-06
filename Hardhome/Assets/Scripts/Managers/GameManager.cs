﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool boolSweptCathedralCleared;
    public static bool boolForsakenFieldCleared;

    public GameObject goHalzenWardGate;

	void Start ()
    {
        boolForsakenFieldCleared = false;
        boolSweptCathedralCleared = false;
	}
	
	void Update ()
    {
		if (boolForsakenFieldCleared &&
            boolSweptCathedralCleared)
        {
            Destroy(goHalzenWardGate);
        }
	}

    public void subLoadScene(string str)
    {
        SceneManager.LoadScene(str);
    }

    public void subQuitGame()
    {
        Application.Quit();
    }
}
