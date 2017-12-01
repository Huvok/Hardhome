using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    GameObject goPlayer;
    Player player;
    CutsceneManager cutsceneManager;
    AudioCrossfader audioCrossfader;
    public AudioClip acToStart;

	void Start ()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        player = goPlayer.GetComponent<Player>();
        cutsceneManager = GameObject.FindGameObjectWithTag("Cutscene Manager").GetComponent<CutsceneManager>();
        audioCrossfader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioCrossfader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (gameObject.name == "Alexa Boss Trigger")
            {
                audioCrossfader.CrossFade(acToStart, .58f, 3);
                cutsceneManager.subTriggerCutscene("Alexa Boss Fight");
                Destroy(gameObject);
            }
        }
    }
}
