using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public RawImage image;
    GameObject area;

    // Para controlar si empieza o no la transición
    bool start = true;
    // Para controlar si la transición es de entrada o salida
    bool isFadeIn = false;
    // Opacidad inicial del cuadrado de transición
    float alpha = 1f;
    // Transición de .8 segundo
    float fadeTime = .8f;

    public AudioSource audioSourceMisc;
    public AudioClip audioClipItemFound;

    void Start ()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        animPlayer = goPlayer.GetComponent<Animator>();
        q = new Queue<pair>();
        dictAnimators = new Dictionary<string, Animator>();
        dictAnimators.Add("Player", animPlayer);
        setHasDescription = new HashSet<string>();
        subPopulateDescriptionSet();
        player = goPlayer.GetComponent<Player>();

        if (SceneManager.GetActiveScene().name == "Scene 1")
        {
            dictAnimators.Add("Alert Message", GameObject.FindGameObjectWithTag("Alert Message").GetComponent<Animator>());
            goAlertMessage = GameObject.FindGameObjectWithTag("Alert Message").transform.GetChild(0).GetComponent<Text>();
            area = area = GameObject.FindGameObjectWithTag("Zone Name");
            subStartGameplay();
        }
        else
        {
            subStartIntro();
        }
    }
	
	void Update ()
    {
        if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Intro_Walking_To_Knight"))
        {
            subMovePlayerToPoint(new Vector2(0f, 0f), new Vector2(-2.93f, 2.93f), animPlayer.GetCurrentAnimatorStateInfo(0).length);
        }

        if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Ending_Walking_To_Gate"))
        {
            subMovePlayerToPoint(goPlayer.transform.position, new Vector2(-2.5f, 111.8f), 5f);
        }
	}

    void OnGUI()
    {
        if (!start) return;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        if (isFadeIn)
        {
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }
        else
        {
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
            if (alpha < 0) start = false;
        }
    }

    void subStartGameplay()
    {
        if (!boolIntroAnimPlayed)
        {
            subLockControls();
            GameObject area = GameObject.FindGameObjectWithTag("Zone Name");
            StartCoroutine(area.GetComponent<Area>().enumShowArea("Ancient Grove"));
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
    }

    void subStartIntro()
    {
        subLockControls();
        animPlayer.Play("Walking_To_Alexa");
        q.Enqueue(new pair("Player", "Running_Away"));
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
        audioSourceMisc.clip = audioClipItemFound;
        audioSourceMisc.Play();
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

    public void subTriggerEvent(string str)
    {
        if (str == "Load Scene-Intro Image")
        {
            StartCoroutine(subLoadScene("Intro Image"));
        }
    }

    IEnumerator subLoadScene(string strScene)
    {
        GameObject[] arrGo = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in arrGo)
        {
            go.GetComponent<Animator>().Play("FL_Following_Kos");
            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1);
        FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(strScene);
    }

    void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    void FadeOut()
    {
        isFadeIn = false;
    }

    public void subTriggerCutscene(string str)
    {
        if (str == "Alexa Boss Fight")
        {
            player.subDisableControls(true);
            image.gameObject.SetActive(true);
            image.GetComponent<Animator>().Play("Image_Fade_In");
            StartCoroutine(subWaitForDialogueEndThen(() => subDisableImageAndTriggerBossFight("Alexa, the Pitiful Marauder"), "Alexa, the Pitiful Marauder 1"));
        }
    }

    private void subDisableImageAndTriggerBossFight(string strBossName)
    {
        StartCoroutine(subFadeImageOutAndTriggerBossFight(strBossName));
    }

    IEnumerator subFadeImageOutAndTriggerBossFight(string strBossName)
    {
        Animator an = image.GetComponent<Animator>();
        an.Play("Image_Fade_Out");
        yield return new WaitForSeconds(1);
        image.gameObject.SetActive(false);
        player.boolDisableControls = false;
        StartCoroutine(area.GetComponent<Area>().enumShowArea(strBossName));
        GameObject.FindGameObjectWithTag("Alexa, the Pitiful Marauder").GetComponent<Unit>().enabled = true;
        GameObject.FindGameObjectWithTag("Alexa, the Pitiful Marauder").GetComponent<AlexaThePitifulMarauder>().enabled = true;
    }

    IEnumerator subWaitForDialogueEndThen(Action action, string str)
    {
        yield return new WaitForSeconds(1);
        subTriggerDialogue(str);
        while (DialogueManager.boolOnDialogue)
        {
            yield return null;
        }

        action.Invoke();
    }

    public void subTriggerEnding()
    {
        animPlayer.Play("Ending_Walking_To_Gate");
        StartCoroutine(subWaitForDialogueEndThen(() => subLoadCredits("Credits"), "Ending 1"));
        AudioSource audioSource = GameObject.FindGameObjectWithTag("Main Camera").GetComponent<AudioSource>();
        StartCoroutine(subFadeAudioSource(audioSource));
    }

    IEnumerator subFadeAudioSource(AudioSource audioSource)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= .005f;
            yield return null;
        }
    }

    IEnumerator subLoadCredits(string strScene)
    {
        yield return new WaitForSeconds(1);
        FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(strScene);
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