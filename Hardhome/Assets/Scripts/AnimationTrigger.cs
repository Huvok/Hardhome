using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    GameObject goPlayer;
    Player player;
    CutsceneManager cutsceneManager;

	void Start ()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        player = goPlayer.GetComponent<Player>();
        cutsceneManager = GameObject.FindGameObjectWithTag("Cutscene Manager").GetComponent<CutsceneManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (gameObject.name == "Alexa Boss Trigger")
            {
                cutsceneManager.subTriggerCutscene("Alexa Boss Fight");
            }
        }
    }
}
