using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

//                                                          //AUTHOR: Hugo Garcia 
//                                                          //Date: 11/18/2017 
//                                                          //PURPOSE: Controller for cutscenes

//====================================================================================================================== 
public class CutsceneManager : MonoBehaviour
{
    public bool boolIntroAnimPlayed;
    Animator animPlayer;
    GameObject goPlayer;
    float t;
    Queue<pair> q;
    Dictionary<string, Animator> dictAnimators;
    Text goAlertMessage;
    HashSet<String> setHasDescription;
    Player player;

	void Start ()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        animPlayer = goPlayer.GetComponent<Animator>();
        q = new Queue<pair>();
        dictAnimators = new Dictionary<string, Animator>();
        dictAnimators.Add("Player", animPlayer);
        dictAnimators.Add("Alert Message", GameObject.FindGameObjectWithTag("Alert Message").GetComponent<Animator>());
        goAlertMessage = GameObject.FindGameObjectWithTag("Alert Message").transform.GetChild(0).GetComponent<Text>();
        setHasDescription = new HashSet<string>();
        subPopulateDescriptionSet();
        player = goPlayer.GetComponent<Player>();
    }
	
	void Update ()
    {
		if (!boolIntroAnimPlayed)
        {
            subLockControls();
            boolIntroAnimPlayed = true;
            animPlayer.Play("Intro_Getting_Up");
            t = 0;
            q.Enqueue(new pair("Player", "Intro_Walking_To_Knight"));
            q.Enqueue(new pair("Player", "Get_Item_Intro_AS"));
            q.Enqueue(new pair("Player", "Player_Idle_Up_No_Cape"));
            q.Enqueue(new pair("Player", "Get_Item_Intro_KC"));
            q.Enqueue(new pair("Player", "Player_Idle_Up"));
            q.Enqueue(new pair("Player", "Player_Dummy"));
        }

        if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Intro_Walking_To_Knight"))
        {
            subMovePlayerToPoint(new Vector2(0f, 0f), new Vector2(-2.93f, 2.93f), animPlayer.GetCurrentAnimatorStateInfo(0).length);
        }
	}

    public void subLockControls()
    {
        player.boolDisableControls = true;
    }

    public void subUnlockControls()
    {
        player.boolDisableControls = false;
    }

    public void subTriggerDialogue(string strDialogue)
    {
        int intDialogue = Int32.Parse(strDialogue.Substring(strDialogue.LastIndexOf(" ") + 1));
        strDialogue = strDialogue.Substring(0, strDialogue.LastIndexOf(" "));
        GameObject.FindGameObjectWithTag(strDialogue).transform.GetChild(intDialogue - 1).GetComponent<DialogueTrigger>().TriggerDialogue();
    }

    public void subTriggerNextOnQueue()
    {
        if (q.Count > 0)
        {
            dictAnimators[q.Peek().first].Play(q.Peek().second);
            q.Dequeue();
        }
    }

    public void subToggleSpriteOn(string str)
    {
        GameObject.Find(str).GetComponent<SpriteRenderer>().enabled = true;
        if (setHasDescription.Contains(str))
        {
            dictAnimators["Alert Message"].SetBool("IsOpen", true);
            goAlertMessage.text = str;
        }
    }

    public void subToggleSpriteOff(string str)
    {
        GameObject.Find(str).GetComponent<SpriteRenderer>().enabled = false;
        if (setHasDescription.Contains(str))
        {
            dictAnimators["Alert Message"].SetBool("IsOpen", false);
        }
    }

    void subMovePlayerToPoint(Vector2 curPos, Vector2 v2, float animLength)
    {
        t += Time.deltaTime / animLength;
        goPlayer.transform.position = Vector3.Lerp(curPos, v2, t);
    }

    private void subPopulateDescriptionSet()
    {
        setHasDescription.Add("Alexa's Scarf");
        setHasDescription.Add("The Order's Cape");
    }
}

public struct pair
{
    public string first, second;

    public pair(string s1, string s2)
    {
        first = s1;
        second = s2;
    }
}