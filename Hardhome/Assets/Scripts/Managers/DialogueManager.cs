using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public static bool boolOnDialogue;
    CutsceneManager cutsceneManager;
    bool boolTriggersAnimation;

	void Start ()
    {
        sentences = new Queue<string>();
        boolOnDialogue = false;
        cutsceneManager = GameObject.FindGameObjectWithTag("Cutscene Manager").GetComponent<CutsceneManager>();
        boolTriggersAnimation = false;
    }
	
    public void StartDialogue (Dialogue dialogue)
    {
        boolOnDialogue = true;
        animator.SetBool("IsOpen", true);
        sentences.Clear();
        nameText.text = dialogue.name;
        boolTriggersAnimation = dialogue.boolTriggersAnimation;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        boolOnDialogue = false;

        if (boolTriggersAnimation)
        {
            cutsceneManager.subTriggerNextOnQueue();
        }
    }
}
