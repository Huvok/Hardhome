using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void subLoadScene1()
    {
        SceneManager.LoadScene("Intro Video");
    }
}
