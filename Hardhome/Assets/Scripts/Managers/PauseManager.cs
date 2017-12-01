using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool boolPause;
    public GameObject goPauseMenu;

	void Start ()
    {
        boolPause = false;
	}

	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.P))
        {
            boolPause = !boolPause;

            if (boolPause)
            {
                goPauseMenu.SetActive(true);
                AudioListener.volume = .1f;
            }
            else
            {
                goPauseMenu.SetActive(false);
                AudioListener.volume = 1f;
            }
        }
	}

    public void subUnpause()
    {
        boolPause = false;
        goPauseMenu.SetActive(false);
        AudioListener.volume = 1f;
    }
}
